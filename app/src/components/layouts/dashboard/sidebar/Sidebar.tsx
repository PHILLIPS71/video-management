'use client'

import type { SidebarQuery$key } from '@/__generated__/SidebarQuery.graphql'
import type { NavigationProps } from '@giantnodes/react'

import { Button, Navigation } from '@giantnodes/react'
import { IconAlbum, IconGauge, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'
import { Suspense } from 'react'
import { graphql, useFragment } from 'react-relay'

import SidebarLibrarySegment from '@/components/layouts/dashboard/sidebar/SidebarLibrarySegment'

const FRAGMENT = graphql`
  fragment SidebarQuery on Query
  @argumentDefinitions(
    first: { type: "Int" }
    after: { type: "String" }
    where: { type: "LibraryFilterInput" }
    order: { type: "[LibrarySortInput!]" }
  ) {
    ...SidebarLibrarySegmentFragment @arguments(first: $first, after: $after, where: $where, order: $order)
  }
`

type SidebarProps = NavigationProps & {
  $key: SidebarQuery$key
}

const Sidebar: React.FC<SidebarProps> = ({ $key, ...rest }) => {
  const fragment = useFragment<SidebarQuery$key>(FRAGMENT, $key)

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

      <Navigation.Segment>
        <Navigation.Title className="flex justify-between items-center">
          Your Libraries
          <Link passHref href="/new">
            <Button color="brand" size="xs">
              <IconAlbum size={16} /> New
            </Button>
          </Link>
        </Navigation.Title>

        <Suspense fallback="LOADING...">
          <SidebarLibrarySegment $key={fragment} />
        </Suspense>
      </Navigation.Segment>

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
