'use client'

import type { page_LibraryDashboardQuery } from '@/__generated__/page_LibraryDashboardQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.context'
import { TranscodeTable } from '@/components/dashboard'

const LibraryDashboard = () => {
  const { library } = useLibraryContext()

  const query = useLazyLoadQuery<page_LibraryDashboardQuery>(
    graphql`
      query page_LibraryDashboardQuery(
        $where: TranscodeFilterInput
        $first: Int
        $after: String
        $order: [TranscodeSortInput!]
      ) {
        ...TranscodeTableFragment @arguments(where: $where, first: $first, after: $after, order: $order)
      }
    `,
    {
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
    }
  )

  return (
    <Card className="max-w-6xl">
      <Card.Header>Tasks</Card.Header>

      <Suspense>
        <TranscodeTable $key={query} />
      </Suspense>
    </Card>
  )
}

export default LibraryDashboard
