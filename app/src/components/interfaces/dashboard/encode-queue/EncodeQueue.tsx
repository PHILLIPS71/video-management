import type { EncodeQueueCancelMutation } from '@/__generated__/EncodeQueueCancelMutation.graphql'
import type {
  EncodeQueueFragment_query$data,
  EncodeQueueFragment_query$key,
} from '@/__generated__/EncodeQueueFragment_query.graphql'
import type { EncodeQueueRefetchQuery } from '@/__generated__/EncodeQueueRefetchQuery.graphql'

import { Button, Link, Table } from '@giantnodes/react'
import { IconCircleX } from '@tabler/icons-react'
import React from 'react'
import { graphql, useMutation, usePaginationFragment } from 'react-relay'

import { EncodeDuration, EncodePercent, EncodeSpeed, EncodeStatus } from '@/components/interfaces/encode'

const FRAGMENT = graphql`
  fragment EncodeQueueFragment_query on Query
  @refetchable(queryName: "EncodeQueueRefetchQuery")
  @argumentDefinitions(where: { type: "EncodeFilterInput" }, first: { type: "Int" }, after: { type: "String" }) {
    encode_queue(where: $where, first: $first, after: $after) @connection(key: "EncodeQueueFragment_encode_queue") {
      edges {
        node {
          id
          status
          file {
            path_info {
              name
            }
          }
          ...EncodeStatusFragment
          ...EncodePercentFragment
          ...EncodeSpeedFragment
          ...EncodeDurationFragment
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

const MUTATION = graphql`
  mutation EncodeQueueCancelMutation($input: Encode_cancelInput!) {
    encode_cancel(input: $input) {
      encode {
        status
      }
      errors {
        ... on DomainError {
          message
        }
        ... on ValidationError {
          message
        }
      }
    }
  }
`

type EncodeEntry = NonNullable<NonNullable<EncodeQueueFragment_query$data['encode_queue']>['edges']>[0]['node']

type EncodeQueueProps = {
  $key: EncodeQueueFragment_query$key
}

const EncodeQueue: React.FC<EncodeQueueProps> = ({ $key }) => {
  const { data, hasNext, isLoadingNext, loadNext } = usePaginationFragment<
    EncodeQueueRefetchQuery,
    EncodeQueueFragment_query$key
  >(FRAGMENT, $key)

  const [commit] = useMutation<EncodeQueueCancelMutation>(MUTATION)

  const cancel = React.useCallback(
    (entry: EncodeEntry) => {
      commit({
        variables: {
          input: {
            encode_id: entry.id,
          },
        },
      })
    },
    [commit]
  )

  return (
    <>
      <Table headingless aria-label="encode queue" size="sm">
        <Table.Head>
          <Table.Column key="name" isRowHeader>
            name
          </Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>

        <Table.Body items={data.encode_queue?.edges ?? []}>
          {(item) => (
            <Table.Row id={item.node.id}>
              <Table.Cell>
                <Link href={`/encode/${item.node.id}`}>{item.node.file.path_info.name}</Link>
              </Table.Cell>
              <Table.Cell>
                <div className="flex flex-row justify-end gap-1">
                  <EncodeStatus $key={item.node} />

                  {item.node.status === 'ENCODING' && (
                    <>
                      <EncodeSpeed $key={item.node} />

                      <EncodePercent $key={item.node} />
                    </>
                  )}

                  {(item.node.status === 'COMPLETED' ||
                    item.node.status === 'CANCELLED' ||
                    item.node.status === 'FAILED') && <EncodeDuration $key={item.node} />}

                  {item.node.status !== 'COMPLETED' &&
                    item.node.status !== 'CANCELLED' &&
                    item.node.status !== 'FAILED' && (
                      <div className="flex flex-row justify-end gap-2">
                        <Button color="neutral" size="xs" onPress={() => cancel(item.node)}>
                          <IconCircleX size={16} />
                        </Button>
                      </div>
                    )}
                </div>
              </Table.Cell>
            </Table.Row>
          )}
        </Table.Body>
      </Table>

      {hasNext && (
        <div className="flex flex-row items-center justify-center p-2">
          <Button isLoading={isLoadingNext} size="xs" onPress={() => loadNext(8)}>
            Show more
          </Button>
        </div>
      )}
    </>
  )
}

export default EncodeQueue
