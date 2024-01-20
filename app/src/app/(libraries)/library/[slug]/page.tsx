'use client'

import type { page_LibraryDashboardQuery } from '@/__generated__/page_LibraryDashboardQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'
import { EncodeTable } from '@/components/dashboard'

const QUERY = graphql`
  query page_LibraryDashboardQuery($where: EncodeFilterInput, $first: Int, $after: String, $order: [EncodeSortInput!]) {
    ...EncodeTableFragment @arguments(where: $where, first: $first, after: $after, order: $order)
  }
`

const LibraryDashboard = () => {
  const { library } = useLibraryContext()

  const query = useLazyLoadQuery<page_LibraryDashboardQuery>(QUERY, {
    first: 8,
    order: [{ created_at: 'DESC' }],
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
    <Card className="max-w-6xl">
      <Card.Header>Tasks</Card.Header>

      <Suspense>
        <EncodeTable $key={query} />
      </Suspense>
    </Card>
  )
}

export default LibraryDashboard
