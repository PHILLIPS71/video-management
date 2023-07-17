'use client'

import type { SerializablePreloadedQuery } from '@/libraries/relay/serializable'
import type { PreloadFetchPolicy, PreloadedQuery } from 'react-relay'
import type { ConcreteRequest, IEnvironment, OperationType } from 'relay-runtime'

import React from 'react'

import { cache } from '@/libraries/relay/network'

const persist = <TRequest extends ConcreteRequest, TQuery extends OperationType>(
  query: SerializablePreloadedQuery<TRequest, TQuery>
) => {
  const key = query.params.id ?? query.params.cacheID

  cache?.set(key, query.variables, query.response)
}

const useSerializableQuery = <TRequest extends ConcreteRequest, TQuery extends OperationType>(
  environment: IEnvironment,
  query: SerializablePreloadedQuery<TRequest, TQuery>,
  fetchPolicy: PreloadFetchPolicy = 'store-or-network'
): PreloadedQuery<TQuery> => {
  React.useMemo(() => {
    persist(query)
  }, [query])

  return {
    environment,
    fetchKey: query.params.id ?? query.params.cacheID,
    fetchPolicy,
    isDisposed: false,
    name: query.params.name,
    kind: 'PreloadedQuery',
    variables: query.variables,
    dispose: () => {},
  }
}

export default useSerializableQuery
