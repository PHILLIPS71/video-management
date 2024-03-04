'use client'

import type { page_ProfilesPageQuery } from '@/__generated__/page_ProfilesPageQuery.graphql'

import { Button, Card, Typography } from '@giantnodes/react'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeProfileTable } from '@/components/interfaces/profiles'

const QUERY = graphql`
  query page_ProfilesPageQuery($first: Int, $after: String, $order: [EncodeProfileSortInput!]) {
    ...EncodeProfileTableFragment @arguments(first: $first, after: $after, order: $order)
  }
`

const EncodeProfileListPage: React.FC = () => {
  const query = useLazyLoadQuery<page_ProfilesPageQuery>(QUERY, {
    first: 25,
    order: [{ name: 'ASC' }],
  })

  return (
    <div className="mx-auto max-w-6xl">
      <div className="flex flex-col gap-6">
        <div className="flex lg:flex-row flex-col gap-2">
          <div className="flex-grow">
            <Typography.HeadingLevel>
              <Typography.Heading as={3}>Encode Profiles</Typography.Heading>
              <Typography.Paragraph variant="subtitle">
                Encode profiles are a set of predefined configurations that will be sent to ffmpeg during an encoding
                operation.
              </Typography.Paragraph>
            </Typography.HeadingLevel>
          </div>

          <div className="mt-auto ml-auto">
            <Button size="xs">Create new profile</Button>
          </div>
        </div>

        <Card>
          <Suspense>
            <EncodeProfileTable $key={query} />
          </Suspense>
        </Card>
      </div>
    </div>
  )
}

export default EncodeProfileListPage
