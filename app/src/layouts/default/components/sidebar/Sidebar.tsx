'use client'

import type { SidebarQuery$key } from '@/__generated__/SidebarQuery.graphql'
import type { NavigationProps } from '@giantnodes/design-system-react'

import { Navigation } from '@giantnodes/design-system-react'
import { IconGauge, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'
import { Suspense } from 'react'
import { graphql, useFragment } from 'react-relay'

import SidebarLibrarySegment from '@/layouts/default/components/sidebar/SidebarLibrarySegment'

type SidebarProps = NavigationProps & {
  $key: SidebarQuery$key
}

const Sidebar: React.FC<SidebarProps> = ({ $key, ...rest }) => {
  const fragment = useFragment<SidebarQuery$key>(
    graphql`
      fragment SidebarQuery on Query
      @argumentDefinitions(first: { type: "Int" }, after: { type: "String" }, order: { type: "[LibrarySortInput!]" }) {
        ...SidebarLibrarySegmentFragment @arguments(first: $first, after: $after, order: $order)
      }
    `,
    $key
  )

  return (
    <Navigation orientation="vertical" size="lg" {...rest}>
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
      </Navigation.Segment>

      <Suspense fallback="LOADING...">
        <SidebarLibrarySegment $key={fragment} />
      </Suspense>

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
