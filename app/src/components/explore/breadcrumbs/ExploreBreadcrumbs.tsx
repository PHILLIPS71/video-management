import type { ExploreBreadcrumbsFragment$key } from '@/__generated__/ExploreBreadcrumbsFragment.graphql'

import { Breadcrumb, Link } from '@giantnodes/react'
import NextLink from 'next/link'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment ExploreBreadcrumbsFragment on FileSystemDirectory {
    library {
      slug
      path_info {
        full_name
        directory_separator_char
      }
    }
    path_info {
      full_name
      directory_separator_char
    }
  }
`

type ExploreBreadcrumbsProps = {
  $key: ExploreBreadcrumbsFragment$key
}

const ExploreBreadcrumbs: React.FC<ExploreBreadcrumbsProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  const directories = React.useMemo<Map<string, string>>(() => {
    const parts = data.path_info.full_name.split(data.path_info.directory_separator_char)

    return parts.reduce<Map<string, string>>((accu, cur) => {
      const previous = Array.from(accu.values()).pop()
      const path = previous ? `${previous}${data.path_info.directory_separator_char}${cur}` : cur

      accu.set(cur, path.replace(data.library.path_info.full_name, ''))

      return accu
    }, new Map<string, string>())
  }, [data.library.path_info.full_name, data.path_info])

  const isBreadcrumbLink = (index: number): boolean => {
    // cannot navigate to the currently viewed directory
    if (directories.size - 1 === index) return false

    const parts = data.library.path_info.full_name.split(data.library.path_info.directory_separator_char).length - 1

    // cannot navigate to a directory that falls within the library root path
    if (parts > index) return false

    return true
  }

  return (
    <nav aria-label="Breadcrumbs">
      <Breadcrumb>
        {Array.from(directories.keys()).map((directory, index) =>
          isBreadcrumbLink(index) ? (
            <Breadcrumb.Item key={directory}>
              <NextLink legacyBehavior href={`/library/${data.library.slug}/explore/${directories.get(directory)}`}>
                <Link href="#">{directory}</Link>
              </NextLink>
            </Breadcrumb.Item>
          ) : (
            <Breadcrumb.Item key={directory}>
              <Link isDisabled href="#">
                {directory}
              </Link>
            </Breadcrumb.Item>
          )
        )}
      </Breadcrumb>
    </nav>
  )
}

export default ExploreBreadcrumbs
