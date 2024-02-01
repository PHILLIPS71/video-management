import type {
  EncodeStatus,
  EncodedTableFragment$data,
  EncodedTableFragment$key,
} from '@/__generated__/EncodedTableFragment.graphql'
import type { EncodedTableRefetchQuery } from '@/__generated__/EncodedTableRefetchQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconTrendingDown, IconTrendingUp } from '@tabler/icons-react'
import dayjs from 'dayjs'
import React from 'react'
import { graphql, usePaginationFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodedTableFragment on Query
  @refetchable(queryName: "EncodedTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "EncodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeSortInput!]" }
  ) {
    complete: encodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodedTableFragment_complete", filters: []) {
      edges {
        node {
          id
          status
          started_at
          completed_at
          file {
            path_info {
              name
            }
          }
          snapshots {
            size
            probed_at
          }
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

type EncodeEntry = NonNullable<NonNullable<EncodedTableFragment$data['complete']>['edges']>[0]['node']

type EncodedTableProps = {
  $key: EncodedTableFragment$key
}

const EncodedTable: React.FC<EncodedTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<EncodedTableRefetchQuery, EncodedTableFragment$key>(
    FRAGMENT,
    $key
  )

  const getStatusColour = (status: EncodeStatus) => {
    switch (status) {
      case 'COMPLETED':
        return 'success'

      case 'CANCELLED':
        return 'neutral'

      case 'FAILED':
        return 'danger'

      default:
        return 'neutral'
    }
  }

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

  const SizeChip = React.useCallback((item: EncodeEntry) => {
    const difference = item.snapshots[item.snapshots.length - 1].size - item.snapshots[0].size
    const increase = difference / item.snapshots[0].size

    return (
      <Chip color={increase > 0 ? 'danger' : 'success'}>
        {increase > 0 ? <IconTrendingUp size={14} /> : <IconTrendingDown size={14} />}

        {percent(Math.abs(increase))}
      </Chip>
    )
  }, [])

  return (
    <>
      <Table headingless aria-label="encode table">
        <Table.Head>
          <Table.Column key="name" isRowHeader>
            name
          </Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>

        <Table.Body items={data.complete?.edges ?? []}>
          {(item) => (
            <Table.Row id={item.node.id}>
              <Table.Cell>
                <Typography.Paragraph>{item.node.file.path_info.name}</Typography.Paragraph>
              </Table.Cell>
              <Table.Cell>
                <div className="flex flex-row items-center justify-end gap-2">
                  <Chip color={getStatusColour(item.node.status)}>{item.node.status.toLowerCase()}</Chip>

                  <Chip color="info">
                    {dayjs.duration(dayjs(item.node.completed_at).diff(item.node.started_at)).format('H[h] m[m] s[s]')}
                  </Chip>

                  {SizeChip(item.node)}
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

export default EncodedTable
