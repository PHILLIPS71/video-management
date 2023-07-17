import type { ConcreteRequest, GraphQLResponse, OperationType, RequestParameters, VariablesOf } from 'relay-runtime'

import { execute } from '@/libraries/relay/network'

export type SerializablePreloadedQuery<TRequest extends ConcreteRequest, TQuery extends OperationType> = {
  params: TRequest['params']
  variables: VariablesOf<TQuery>
  response: GraphQLResponse
}

// Call into raw network fetch to get serializable GraphQL query response the response will be sent to
// the client to "warm" the QueryResponseCache to avoid the client fetches.
export const getSerializableQuery = async <TRequest extends ConcreteRequest, TQuery extends OperationType>(
  params: RequestParameters,
  variables: VariablesOf<TQuery>
): Promise<SerializablePreloadedQuery<TRequest, TQuery>> => {
  const response = await execute(params, variables)

  return {
    params,
    variables,
    response,
  }
}
