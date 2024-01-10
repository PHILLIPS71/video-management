import type { TranscodeTable_TranscodeCancelMutation } from '@/__generated__/TranscodeTable_TranscodeCancelMutation.graphql'
import type {
  TranscodeStatus,
  TranscodeTableFragment$data,
  TranscodeTableFragment$key,
} from '@/__generated__/TranscodeTableFragment.graphql'
import type { TranscodeTablePaginationQuery } from '@/__generated__/TranscodeTablePaginationQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconProgressX } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useMutation, usePaginationFragment, useSubscription } from 'react-relay'

const TranscodeTableFragment = graphql`
  fragment TranscodeTableFragment on Query
  @refetchable(queryName: "TranscodeTablePaginationQuery")
  @argumentDefinitions(
    where: { type: "TranscodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[TranscodeSortInput!]" }
  ) {
    transcodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "TranscodeTableFragment_transcodes", filters: []) {
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

type TranscodeTableProps = {
  $key: TranscodeTableFragment$key
}

type TranscodeEntry = NonNullable<NonNullable<TranscodeTableFragment$data['transcodes']>['edges']>[0]['node']

const TranscodeTable: React.FC<TranscodeTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<TranscodeTablePaginationQuery, TranscodeTableFragment$key>(
    TranscodeTableFragment,
    $key
  )

  const [commit] = useMutation<TranscodeTable_TranscodeCancelMutation>(graphql`
    mutation TranscodeTable_TranscodeCancelMutation($input: File_transcode_cancelInput!) {
      file_transcode_cancel(input: $input) {
        transcode {
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
  `)

  useSubscription({
    subscription: graphql`
      subscription TranscodeTableSubscription {
        transcode_speed_change {
          percent
          speed {
            frames
            bitrate
            scale
          }
        }
      }
    `,
    variables: {},
  })

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

  const getStatusColour = (status: TranscodeStatus) => {
    switch (status) {
      case 'SUBMITTED':
        return 'info'

      case 'QUEUED':
        return 'info'

      case 'TRANSCODING':
        return 'success'

      case 'CANCELLING':
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
    (entry: TranscodeEntry) => {
      commit({
        variables: {
          input: {
            file_id: entry.file.id,
            transcode_id: entry.id,
          },
        },
      })
    },
    [commit]
  )

  const render = React.useCallback(
    (item: TranscodeEntry, key: React.Key) => {
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

              {item.status !== 'COMPLETED' && item.status !== 'CANCELLING' && item.status !== 'CANCELLED' && (
                <div
                  className="cursor-pointer text-shark-200"
                  onClick={() => cancel(item)}
                  onKeyDown={() => cancel(item)}
                >
                  <IconProgressX size={16} />
                </div>
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
        <Table.Body items={data.transcodes?.edges ?? []}>
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

export default TranscodeTable
