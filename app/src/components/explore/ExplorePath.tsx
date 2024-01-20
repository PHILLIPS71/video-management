import type { ExplorePathFragment$key } from '@/__generated__/ExplorePathFragment.graphql'

import { Breadcrumb, Link } from '@giantnodes/react'
import NextLink from 'next/link'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'

const FRAGMENT = graphql`
  fragment ExplorePathFragment on FileSystemDirectory {
    path_info {
      full_name
      directory_separator_char
    }
  }
`

type ExplorePathProps = {
  $key: ExplorePathFragment$key
}

const ExplorePath: React.FC<ExplorePathProps> = ({ $key }) => {
  const { library } = useLibraryContext()

  const data = useFragment(FRAGMENT, $key)

  const directories = React.useMemo<Map<string, string>>(() => {
    const parts = data.path_info.full_name.split(data.path_info.directory_separator_char)

    return parts.reduce<Map<string, string>>((accu, cur) => {
      const previous = Array.from(accu.values()).pop()
      const path = previous ? `${previous}${data.path_info.directory_separator_char}${cur}` : cur

      accu.set(cur, path.replace(library.path_info.full_name, ''))

      return accu
    }, new Map<string, string>())
  }, [library.path_info.full_name, data.path_info])

  const isBreadcrumbLink = (index: number): boolean => {
    // cannot navigate to the currently viewed directory
    if (directories.size - 1 === index) return false

    const parts = library.path_info.full_name.split(library.path_info.directory_separator_char).length - 1

    // cannot navigate to a directory that falls within the library root path
    if (parts > index) return false

    return true
  }

  return (
    <Breadcrumb>
      {Array.from(directories.keys()).map((directory, index) =>
        isBreadcrumbLink(index) ? (
          <Breadcrumb.Item key={directory}>
            <NextLink legacyBehavior href={`/library/${library.slug}/explore/${directories.get(directory)}`}>
              <Link className="font-semibold">{directory}</Link>
            </NextLink>
          </Breadcrumb.Item>
        ) : (
          <Breadcrumb.Item key={directory} disabled>
            {directory}
          </Breadcrumb.Item>
        )
      )}
    </Breadcrumb>
  )
}

export default ExplorePath
