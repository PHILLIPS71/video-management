'use client'

import type { page_LibrarySlugExploreQuery } from '@/__generated__/page_LibrarySlugExploreQuery.graphql'

import { Card, Typography } from '@giantnodes/react'
import { notFound } from 'next/navigation'
import React, { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'
import {
  ExploreBreadcrumbs,
  ExploreContext,
  ExploreControls,
  ExploreTable,
  useExplore,
} from '@/components/interfaces/explore'
import { ResolutionWidget } from '@/components/widgets'

const QUERY = graphql`
  query page_LibrarySlugExploreQuery($where: FileSystemDirectoryFilterInput, $order: [FileSystemEntrySortInput!]) {
    file_system_directory(where: $where) {
      id
      ...ExploreTableFragment @arguments(order: $order)
      ...ExploreBreadcrumbsFragment
      ...ExploreControlsFragment
    }
  }
`

type LibraryExplorePageProps = {
  params: {
    slug: string
    path?: string[]
  }
}

const LibraryExplorePage: React.FC<LibraryExplorePageProps> = ({ params }) => {
  const { library } = useLibraryContext()

  const path = React.useMemo<string>(() => {
    const separator = library.path_info.directory_separator_char
    let location = library.path_info.full_name

    if (params.path && params.path.length > 0) {
      location = `${location}${separator}${decodeURI(params.path.join(separator))}`
    }

    return location
  }, [library.path_info.directory_separator_char, library.path_info.full_name, params.path])

  const query = useLazyLoadQuery<page_LibrarySlugExploreQuery>(QUERY, {
    where: {
      and: [
        {
          library: {
            id: {
              eq: library.id,
            },
          },
        },
        {
          path_info: {
            full_name: {
              eq: path,
            },
          },
        },
      ],
    },
    order: [
      {
        size: 'ASC',
      },
    ],
  })

  if (query.file_system_directory == null) {
    notFound()
  }

  const context = useExplore({ directory: query.file_system_directory.id })

  return (
    <div className="flex lg:flex-row flex-col gap-2">
      <ExploreContext.Provider value={context}>
        <div className="flex flex-col flex-1 gap-2">
          <Card transparent>
            <Card.Header>
              <Suspense fallback="LOADING...">
                <ExploreBreadcrumbs $key={query.file_system_directory} />
              </Suspense>
            </Card.Header>
          </Card>

          <Card transparent>
            <Card.Header>
              <Suspense fallback="LOADING...">
                <ExploreControls $key={query.file_system_directory} />
              </Suspense>
            </Card.Header>
          </Card>

          <Card>
            <Suspense fallback="LOADING...">
              <ExploreTable $key={query.file_system_directory} />
            </Suspense>
          </Card>
        </div>
      </ExploreContext.Provider>

      <div className="flex flex-col gap-2">
        <Card className="h-fit lg:w-80">
          <Card.Header>
            <Typography.Text as="strong">Resolution</Typography.Text>
          </Card.Header>

          <Card.Body>
            <Suspense fallback="LOADING...">
              <ResolutionWidget directory={query.file_system_directory.id} />
            </Suspense>
          </Card.Body>
        </Card>
      </div>
    </div>
  )
}

export default LibraryExplorePage
