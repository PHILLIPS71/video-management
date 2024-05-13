'use client'

import type { page_DashboardPageQuery } from '@/__generated__/page_DashboardPageQuery.graphql'

import { Card } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeQueue } from '@/components/interfaces/dashboard'

const QUERY = graphql`
  query page_DashboardPageQuery($first: Int, $after: String) {
    ...EncodeQueueFragment_query @arguments(first: $first, after: $after)
  }
`

const DashboardPage = () => {
  const query = useLazyLoadQuery<page_DashboardPageQuery>(QUERY, {
    first: 8,
  })

  return (
    <div className="flex flex-col gap-3 h-full">
      <Card className="max-w-4xl">
        <Card.Header>Queue</Card.Header>

        <Suspense>
          <Card.Body className="p-0">
            <EncodeQueue $key={query} />
          </Card.Body>
        </Suspense>
      </Card>
    </div>
  )
}

export default DashboardPage
