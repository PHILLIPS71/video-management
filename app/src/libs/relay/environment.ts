import { Environment, RecordSource, Store } from 'relay-runtime'

import * as network from '@/libs/relay/network'

const IS_SERVER = typeof window === typeof undefined

const create = () =>
  new Environment({
    network: network.create(),
    store: new Store(RecordSource.create()),
    isServer: IS_SERVER,
  })

export const environment = create()

export const getEnvironment = () => {
  if (IS_SERVER) {
    return create()
  }

  return environment
}
