import type { CacheConfig, GraphQLResponse, RequestParameters, Variables } from 'relay-runtime'

import { Network, QueryResponseCache } from 'relay-runtime'

const HTTP_ENDPOINT = `${process.env.NEXT_PUBLIC_API_URI}/graphql`
const CACHE_TTL = 5 * 1000

const IS_SERVER = typeof window === typeof undefined

const cache: QueryResponseCache | null = IS_SERVER ? null : new QueryResponseCache({ size: 100, ttl: CACHE_TTL })

const execute = async (request: RequestParameters, variables: Variables): Promise<GraphQLResponse> => {
  const response = await fetch(HTTP_ENDPOINT, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      query: request.text,
      variables,
    }),
  })

  const json = await response.json()
  return json
}

export const create = () => {
  const fetch = async (params: RequestParameters, variables: Variables, config: CacheConfig) => {
    const { force } = config

    const isQuery = params.operationKind === 'query'
    const key = params.id ?? params.cacheID

    if (cache != null && isQuery && !force) {
      const cached = cache.get(key, variables)

      if (cached != null) {
        return Promise.resolve(cached)
      }
    }

    return execute(params, variables)
  }

  const network = Network.create(fetch)
  return network
}
