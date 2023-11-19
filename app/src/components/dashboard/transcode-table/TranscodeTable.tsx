import type { TranscodeTableFragment$key } from '@/__generated__/TranscodeTableFragment.graphql'
import type { TranscodeTablePaginationQuery } from '@/__generated__/TranscodeTablePaginationQuery.graphql'

import { Button, Table } from '@giantnodes/react'
import { graphql, usePaginationFragment } from 'react-relay'

import TranscodeTableRow from '@/components/dashboard/transcode-table/TranscodeTableRow'

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
          ...TranscodeTableRowFragment
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

  return (
    <>
      <Table>
        <Table.Body>
          {data.transcodes?.edges?.map((transcode) => <TranscodeTableRow $key={transcode.node} />)}
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
