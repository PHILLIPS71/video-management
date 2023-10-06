import type { ExploreTableFragment$key } from '@/__generated__/ExploreTableFragment.graphql'

import { Table } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

import { ExploreTableDirectory, ExploreTableFile } from '@/components/explore/table'

type ExploreTableProps = {
  $key: ExploreTableFragment$key
}

const ExploreTable: React.FC<ExploreTableProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment ExploreTableFragment on FileSystemDirectory
      @argumentDefinitions(order: { type: "[FileSystemEntrySortInput!]" }) {
        entries(order: $order) {
          __typename
          ... on FileSystemDirectory {
            ...ExploreTableDirectoryFragment
          }
          ... on FileSystemFile {
            ...ExploreTableFileFragment
          }
        }
      }
    `,
    $key
  )

  return (
    <Table>
      <Table.Body>
        {data.entries?.map((entry) => (
          <>
            {entry?.__typename === 'FileSystemDirectory' && <ExploreTableDirectory $key={entry} />}
            {entry?.__typename === 'FileSystemFile' && <ExploreTableFile $key={entry} />}
          </>
        ))}
      </Table.Body>
    </Table>
  )
}

export default ExploreTable
