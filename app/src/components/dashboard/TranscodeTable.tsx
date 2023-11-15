import type { TranscodeStatus, TranscodeTableFragment$key } from '@/__generated__/TranscodeTableFragment.graphql'
import type { TranscodeTablePaginationQuery } from '@/__generated__/TranscodeTablePaginationQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { graphql, usePaginationFragment } from 'react-relay'

type TranscodeTableProps = {
  $key: TranscodeTableFragment$key
}

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
          file {
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

const TranscodeTable: React.FC<TranscodeTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<TranscodeTablePaginationQuery, TranscodeTableFragment$key>(
    TranscodeTableFragment,
    $key
  )

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

      case 'FAILED':
        return 'danger'

      default:
        return 'danger'
    }
  }

  return (
    <>
      <Table>
        <Table.Body>
          {data.transcodes?.edges?.map((transcode) => (
            <Table.Row>
              <Table.Data>
                <div className="flex flex-row items-center gap">
                  <Typography.Text>{transcode.node.file.path_info.name}</Typography.Text>
                </div>
              </Table.Data>

              <Table.Data align="right">
                <div className="flex flex-row items-center justify-end gap-2">
                  {transcode.node.percent != null && <Chip color="info">{percent(transcode.node.percent)}</Chip>}
                  <Chip color={getStatusColour(transcode.node.status)}>{transcode.node.status.toLowerCase()}</Chip>
                </div>
              </Table.Data>
            </Table.Row>
          ))}
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
