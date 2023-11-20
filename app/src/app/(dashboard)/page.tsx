'use client'

import type { page_DashboardPageQuery } from '@/__generated__/page_DashboardPageQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { TranscodeTable } from '@/components/dashboard'

const DashboardPage = () => {
  const query = useLazyLoadQuery<page_DashboardPageQuery>(
    graphql`
      query page_DashboardPageQuery(
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
    }
  )

  return (
    <Card className="max-w-6xl">
      <Card.Header>Transcoding</Card.Header>

      <Suspense>
        <TranscodeTable $key={query} />
      </Suspense>
    </Card>
  )
}

export default DashboardPage
