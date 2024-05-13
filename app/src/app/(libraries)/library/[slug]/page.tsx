'use client'

import type { page_LibraryDashboardQuery } from '@/__generated__/page_LibraryDashboardQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'
import { EncodeQueue } from '@/components/interfaces/dashboard'

const QUERY = graphql`
  query page_LibraryDashboardQuery($where: EncodeFilterInput, $first: Int, $after: String) {
    ...EncodeQueueFragment_query @arguments(where: $where, first: $first, after: $after)
  }
`

const LibraryDashboard = () => {
  const { library } = useLibraryContext()

  const query = useLazyLoadQuery<page_LibraryDashboardQuery>(QUERY, {
    first: 8,
    where: {
      file: {
        library: {
          id: {
            eq: library.id,
          },
        },
      },
    },
  })

  return (
    <Card className="max-w-6xl mx-auto">
      <Card.Header>Tasks</Card.Header>

      <Suspense>
        <EncodeQueue $key={query} />
      </Suspense>
    </Card>
  )
}

export default LibraryDashboard
