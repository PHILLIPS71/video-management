'use client'

import type { page_LibrarySlugExploreQuery } from '@/__generated__/page_LibrarySlugExploreQuery.graphql'

import { Card } from '@giantnodes/design-system-react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.context'
import LibraryExploreTable from '@/components/LibraryExploreTable'

const LibrarySlugExplorePage: React.FC = () => {
  const { library } = useLibraryContext()
  const query = useLazyLoadQuery<page_LibrarySlugExploreQuery>(
    graphql`
      query page_LibrarySlugExploreQuery(
        $first: Int
        $after: String
        $where: FileSystemFileFilterInput
        $order: [FileSystemFileSortInput!]
      ) {
        ...LibraryExploreTableFragment @arguments(first: $first, after: $after, where: $where, order: $order)
      }
    `,
    {
      first: 20,
      where: {
        path_info: {
          directory_path: {
            startsWith: library.path_info.full_name,
          },
        },
      },
      order: [
        {
          path_info: { full_name: 'ASC' },
        },
      ],
    }
  )

  return (
    <Card>
      <Card.Body>
        <LibraryExploreTable $key={query} />
      </Card.Body>
    </Card>
  )
}

export default LibrarySlugExplorePage
