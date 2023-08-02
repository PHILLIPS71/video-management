import type { LibraryExploreTableFragment$key } from '@/__generated__/LibraryExploreTableFragment.graphql'
import type { LibraryExploreTablePaginationQuery } from '@/__generated__/LibraryExploreTablePaginationQuery.graphql'

import { Table, Typography } from '@giantnodes/design-system-react'
import { IconFile } from '@tabler/icons-react'
import { filesize } from 'filesize'
import { useInView } from 'react-intersection-observer'
import { graphql, usePaginationFragment } from 'react-relay'

type LibraryExploreTableProps = {
  $key: LibraryExploreTableFragment$key
}

const LibraryExploreTable: React.FC<LibraryExploreTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<
    LibraryExploreTablePaginationQuery,
    LibraryExploreTableFragment$key
  >(
    graphql`
      fragment LibraryExploreTableFragment on Query
      @refetchable(queryName: "LibraryExploreTablePaginationQuery")
      @argumentDefinitions(
        first: { type: "Int" }
        after: { type: "String" }
        where: { type: "FileSystemFileFilterInput" }
        order: { type: "[FileSystemFileSortInput!]" }
      ) {
        file_system_entries(first: $first, after: $after, where: $where, order: $order)
          @connection(key: "LibraryExploreTable_file_system_entries", filters: ["where", "order"]) {
          edges {
            node {
              id
              size
              path_info {
                name
              }
            }
          }
          pageInfo {
            hasNextPage
          }
        }
      }
    `,
    $key
  )

  const { ref } = useInView({
    onChange: (inview) => {
      if (inview && hasNext) {
        loadNext(20)
      }
    },
  })

  return (
    <>
      <Table>
        <Table.Head>
          <Table.Row>
            <Table.Header>Name</Table.Header>
            <Table.Header>Title</Table.Header>
          </Table.Row>
        </Table.Head>
        <Table.Body>
          {data.file_system_entries?.edges?.map((file) => (
            <Table.Row key={file.node.id}>
              <Table.Data>
                <div className="flex flex-row items-center gap-2">
                  <IconFile size={20} />
                  <Typography.Text>{file.node.path_info.name}</Typography.Text>
                </div>
              </Table.Data>
              <Table.Data>
                <Typography.Text>{filesize(file.node.size)}</Typography.Text>
              </Table.Data>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>

      {hasNext && <p ref={ref}>Loading Spinner...</p>}
    </>
  )
}

export default LibraryExploreTable
