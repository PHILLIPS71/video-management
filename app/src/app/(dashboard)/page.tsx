'use client'

import type { page_DashboardPageQuery } from '@/__generated__/page_DashboardPageQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeTable } from '@/components/dashboard'

const DashboardPage = () => {
  const query = useLazyLoadQuery<page_DashboardPageQuery>(
    graphql`
      query page_DashboardPageQuery(
        $where: EncodeFilterInput
        $first: Int
        $after: String
        $order: [EncodeSortInput!]
      ) {
        ...EncodeTableFragment @arguments(where: $where, first: $first, after: $after, order: $order)
      }
    `,
    {
      first: 8,
      order: [{ created_at: 'DESC' }],
    }
  )

  return (
    <Card className="max-w-6xl">
      <Card.Header>Encoding</Card.Header>

      <Suspense>
        <EncodeTable $key={query} />
      </Suspense>
    </Card>
  )
}

export default DashboardPage
