import type { CacheConfig, GraphQLResponse, RequestParameters, SubscribeFunction, Variables } from 'relay-runtime'

import { createClient } from 'graphql-ws'
import { Network, Observable, QueryResponseCache } from 'relay-runtime'

const HTTP_ENDPOINT = `${process.env.NEXT_PUBLIC_API_URI}/graphql`
const WS_ENDPOINT = `${process.env.NEXT_PUBLIC_API_WS_URI}/graphql`
const CACHE_TTL = 5 * 1000

const IS_SERVER = typeof window === typeof undefined

const websocket = createClient({
  url: WS_ENDPOINT,
})

export const cache: QueryResponseCache | null = IS_SERVER ? null : new QueryResponseCache({ size: 100, ttl: CACHE_TTL })

export const execute = async (parameters: RequestParameters, variables: Variables): Promise<GraphQLResponse> => {
  const response = await fetch(HTTP_ENDPOINT, {
    method: 'POST',
    cache: 'no-store',
    next: {
      revalidate: 0,
    },
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      query: parameters.text,
      variables,
    }),
  })

  const json = await response.json()

  return json
}

export const subscribe: SubscribeFunction = (
  parameters: RequestParameters,
  variables?: Record<string, unknown> | null
) =>
  Observable.create((sink) =>
    websocket.subscribe(
      {
        operationName: parameters.name,
        query: parameters.text as string,
        variables,
      },
      sink
    )
  )

export const create = () => {
  const fetch = async (parameters: RequestParameters, variables: Variables, config: CacheConfig) => {
    const { force } = config

    const isQuery = parameters.operationKind === 'query'
    const key = parameters.id ?? parameters.cacheID

    if (cache != null && isQuery && !force) {
      const cached = cache.get(key, variables)

      if (cached != null) {
        return Promise.resolve(cached)
      }
    }

    return execute(parameters, variables)
  }

  const network = Network.create(fetch, subscribe)

  return network
}
