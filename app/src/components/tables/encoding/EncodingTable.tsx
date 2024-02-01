import type { EncodingTable_EncodeCancelMutation } from '@/__generated__/EncodingTable_EncodeCancelMutation.graphql'
import type {
  EncodeStatus,
  EncodingTableFragment$data,
  EncodingTableFragment$key,
} from '@/__generated__/EncodingTableFragment.graphql'
import type { EncodingTableRefetchQuery } from '@/__generated__/EncodingTableRefetchQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconProgressX } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useMutation, usePaginationFragment, useSubscription } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodingTableFragment on Query
  @refetchable(queryName: "EncodingTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "EncodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeSortInput!]" }
  ) {
    incomplete: encodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodingTableFragment_incomplete", filters: []) {
      edges {
        node {
          id
          status
          percent
          speed {
            frames
            bitrate
            scale
          }
          file {
            path_info {
              name
            }
          }
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

const MUTATION = graphql`
  mutation EncodingTable_EncodeCancelMutation($input: Encode_cancelInput!) {
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

const SUBSCRIPTION = graphql`
  subscription EncodingTableSubscription {
    encode_speed_change {
      percent
      speed {
        frames
        bitrate
        scale
      }
    }
  }
`

type EncodeEntry = NonNullable<NonNullable<EncodingTableFragment$data['incomplete']>['edges']>[0]['node']

type EncodingTableProps = {
  $key: EncodingTableFragment$key
}

const EncodingTable: React.FC<EncodingTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<EncodingTableRefetchQuery, EncodingTableFragment$key>(
    FRAGMENT,
    $key
  )

  const [commit] = useMutation<EncodingTable_EncodeCancelMutation>(MUTATION)

  useSubscription({
    subscription: SUBSCRIPTION,
    variables: {},
  })

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

  const getStatusColour = (status: EncodeStatus) => {
    switch (status) {
      case 'SUBMITTED':
        return 'info'

      case 'QUEUED':
        return 'info'

      case 'ENCODING':
        return 'success'

      case 'DEGRADED':
        return 'warning'

      default:
        return 'neutral'
    }
  }

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
      <Table headingless aria-label="encode table">
        <Table.Head>
          <Table.Column key="name" isRowHeader>
            name
          </Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>

        <Table.Body items={data.incomplete?.edges ?? []}>
          {(item) => (
            <Table.Row id={item.node.id}>
              <Table.Cell>
                <Typography.Paragraph>{item.node.file.path_info.name}</Typography.Paragraph>
              </Table.Cell>
              <Table.Cell>
                <div className="flex flex-row items-center justify-end gap-2">
                  {item.node.speed != null && (
                    <>
                      <Chip color="info">{item.node.speed.frames} fps</Chip>

                      <Chip color="info">
                        {filesize(item.node.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s
                      </Chip>

                      <Chip color="info">{item.node.speed.scale.toFixed(2)}x</Chip>
                    </>
                  )}

                  {item.node.percent != null && <Chip color="info">{percent(item.node.percent)}</Chip>}

                  <Chip color={getStatusColour(item.node.status)}>{item.node.status.toLowerCase()}</Chip>

                  <Button color="neutral" size="xs" variant="blank" onClick={() => cancel(item.node)}>
                    <IconProgressX size={16} />
                  </Button>
                </div>
              </Table.Cell>
            </Table.Row>
          )}
        </Table.Body>
      </Table>

      {hasNext && (
        <div className="flex flex-row items-center justify-center p-2">
          <Button size="xs" onClick={() => loadNext(8)}>
            Show more
          </Button>
        </div>
      )}
    </>
  )
}

export default EncodingTable
