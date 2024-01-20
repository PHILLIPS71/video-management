import type {
  EncodeStatus,
  EncodeTable_EncodeCancelMutation,
} from '@/__generated__/EncodeTable_EncodeCancelMutation.graphql'
import type { EncodeTableFragment$data, EncodeTableFragment$key } from '@/__generated__/EncodeTableFragment.graphql'
import type { EncodeTablePaginationQuery } from '@/__generated__/EncodeTablePaginationQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconProgressX } from '@tabler/icons-react'
import dayjs from 'dayjs'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useMutation, usePaginationFragment, useSubscription } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodeTableFragment on Query
  @refetchable(queryName: "EncodeTablePaginationQuery")
  @argumentDefinitions(
    where: { type: "EncodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeSortInput!]" }
  ) {
    encodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodeTableFragment_encodes", filters: []) {
      edges {
        node {
          id
          status
          percent
          started_at
          completed_at
          speed {
            frames
            bitrate
            scale
          }
          file {
            id
            library {
              name
            }
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
  mutation EncodeTable_EncodeCancelMutation($input: File_encode_cancelInput!) {
    file_encode_cancel(input: $input) {
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
  subscription EncodeTableSubscription {
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

type EncodeTableProps = {
  $key: EncodeTableFragment$key
}

type EncodeEntry = NonNullable<NonNullable<EncodeTableFragment$data['encodes']>['edges']>[0]['node']

const EncodeTable: React.FC<EncodeTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<EncodeTablePaginationQuery, EncodeTableFragment$key>(
    FRAGMENT,
    $key
  )

  const [commit] = useMutation<EncodeTable_EncodeCancelMutation>(MUTATION)

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

      case 'FAILED':
        return 'danger'

      case 'CANCELLED':
        return 'neutral'

      case 'COMPLETED':
        return 'success'

      default:
        return 'danger'
    }
  }

  const cancel = React.useCallback(
    (entry: EncodeEntry) => {
      commit({
        variables: {
          input: {
            file_id: entry.file.id,
            encode_id: entry.id,
          },
        },
      })
    },
    [commit]
  )

  const render = React.useCallback(
    (item: EncodeEntry, key: React.Key) => {
      switch (key) {
        case 'name':
          return <Typography.Text>{item.file.path_info.name}</Typography.Text>

        case 'stats':
          return (
            <div className="flex flex-row items-center justify-end gap-2">
              {item.status !== 'COMPLETED' && item.status !== 'CANCELLED' && (
                <>
                  {item.speed != null && (
                    <>
                      <Chip color="info">{item.speed.frames} fps</Chip>
                      <Chip color="info">{filesize(item.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s</Chip>
                      <Chip color="info">{item.speed.scale.toFixed(2)}x</Chip>
                    </>
                  )}

                  {item.percent != null && <Chip color="info">{percent(item.percent)}</Chip>}
                </>
              )}

              <Chip color={getStatusColour(item.status)}>{item.status.toLowerCase()}</Chip>

              {item.status === 'COMPLETED' && (
                <Chip color="info">
                  {dayjs.duration(dayjs(item.completed_at).diff(item.started_at)).format('H[h] m[m] s[s]')}
                </Chip>
              )}

              {item.status !== 'COMPLETED' && item.status !== 'CANCELLED' && (
                <Button color="neutral" size="xs" variant="blank" onClick={() => cancel(item)}>
                  <IconProgressX size={16} />
                </Button>
              )}
            </div>
          )

        default:
          throw new Error(`the key '${key}' is unexpected`)
      }
    },
    [cancel]
  )

  return (
    <>
      <Table headingless>
        <Table.Head>
          <Table.Column key="name">name</Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>
        <Table.Body items={data.encodes?.edges ?? []}>
          {(item) => (
            <Table.Row key={item.node.id}>{(key) => <Table.Cell>{render(item.node, key)}</Table.Cell>}</Table.Row>
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

export default EncodeTable
