'use client'

import type { page_DashboardPageQuery } from '@/__generated__/page_DashboardPageQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodedTable, EncodingTable } from '@/components/tables'

const DASHBOARD_QUERY = graphql`
  query page_DashboardPageQuery($first: Int, $after: String, $order: [EncodeSortInput!]) {
    ...EncodingTableFragment
      @arguments(
        where: { status: { nin: [COMPLETED, FAILED, CANCELLED] } }
        first: $first
        after: $after
        order: $order
      )

    ...EncodedTableFragment
      @arguments(where: { status: { in: [COMPLETED, FAILED, CANCELLED] } }, first: $first, after: $after, order: $order)
  }
`

const DashboardPage = () => {
  const query = useLazyLoadQuery<page_DashboardPageQuery>(DASHBOARD_QUERY, {
    first: 8,
    order: [{ created_at: 'DESC' }],
  })

  return (
    <div className="flex flex-col gap-4">
      <Card className="max-w-4xl">
        <Card.Header>Processing</Card.Header>

        <Suspense>
          <EncodingTable $key={query} />
        </Suspense>
      </Card>

      <Card className="max-w-4xl">
        <Card.Header>Completed</Card.Header>

        <Suspense>
          <EncodedTable $key={query} />
        </Suspense>
      </Card>
    </div>
  )
}

export default DashboardPage
