'use client'

import type { page_LibrarySlugExploreQuery } from '@/__generated__/page_LibrarySlugExploreQuery.graphql'

import { Card, Typography } from '@giantnodes/react'
import { notFound } from 'next/navigation'
import React, { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.context'
import ExploreCodecs from '@/components/explore/ExploreCodecs'
import ExploreContainers from '@/components/explore/ExploreContainers'
import ExploreControls from '@/components/explore/ExploreControls'
import ExplorePath from '@/components/explore/ExplorePath'
import ExploreTable from '@/components/explore/table/ExploreTable'

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

  const query = useLazyLoadQuery<page_LibrarySlugExploreQuery>(
    graphql`
      query page_LibrarySlugExploreQuery($where: FileSystemDirectoryFilterInput, $order: [FileSystemEntrySortInput!]) {
        file_system_directory(where: $where) {
          ...ExplorePathFragment
          ...ExploreControlsFragment
          ...ExploreTableFragment @arguments(order: $order)
        }
      }
    `,
    {
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
    }
  )

  if (query.file_system_directory == null) {
    notFound()
  }

  return (
    <div className="flex lg:flex-row flex-col gap-2">
      <div className="flex flex-col flex-1 gap-2">
        <Card transparent>
          <Card.Header>
            <Suspense fallback="LOADING...">
              <ExplorePath $key={query.file_system_directory} />
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

      <div className="flex flex-col gap-2">
        <Card className="h-fit lg:w-80">
          <Card.Header>
            <Typography.Text as="strong">Containers</Typography.Text>
          </Card.Header>

          <Card.Body>
            <ExploreContainers />
          </Card.Body>
        </Card>

        <Card className="h-fit lg:w-80">
          <Card.Header>
            <Typography.Text as="strong">Codecs</Typography.Text>
          </Card.Header>

          <Card.Body>
            <ExploreCodecs />
          </Card.Body>
        </Card>
      </div>
    </div>
  )
}

export default LibraryExplorePage
