'use client'

import type { SidebarLibrarySegmentPaginationQuery } from '@/__generated__/SidebarLibrarySegmentPaginationQuery.graphql'
import type { NavigationProps } from '@giantnodes/design-system-react'

import { Navigation } from '@giantnodes/design-system-react'
import { IconFolders, IconGauge, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'
import { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import SidebarLibrarySegment from '@/layouts/components/sidebar/SidebarLibrarySegment'

const SidebarQuery = graphql`
  query SidebarQuery($cursor: String, $count: Int) {
    ...SidebarLibrarySegmentFragment @arguments(cursor: $cursor, count: $count)
  }
`

export type SidebarProps = NavigationProps

const Sidebar: React.FC<SidebarProps> = ({ library, ...rest }) => {
  const libraries = useLazyLoadQuery<SidebarLibrarySegmentPaginationQuery>(SidebarQuery, {
    count: 8,
  })

  return (
    <Navigation orientation="vertical" {...rest}>
      <Navigation.Brand>
        <Image priority alt="giantnodes logo" height={40} src="/images/giantnodes-logo.png" width={128} />
      </Navigation.Brand>

      <Navigation.Segment>
        <Navigation.Item>
          <Link legacyBehavior passHref href="/">
            <Navigation.Link>
              <IconGauge strokeWidth={1.5} /> Dashboard
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Item>
          <Link legacyBehavior passHref href="/explore">
            <Navigation.Link>
              <IconFolders strokeWidth={1.5} /> Explore
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>

      <SidebarLibrarySegment libraries={libraries} />

      <Navigation.Segment className="mt-auto">
        <Navigation.Item>
          <Link legacyBehavior passHref href="/">
            <Navigation.Link>
              <IconSettings strokeWidth={1.5} /> Settings
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Sidebar
