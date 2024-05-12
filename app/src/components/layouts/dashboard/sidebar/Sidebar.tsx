'use client'

import type { SidebarQuery$key } from '@/__generated__/SidebarQuery.graphql'
import type { NavigationProps } from '@giantnodes/react'

import { Button, Link, Navigation } from '@giantnodes/react'
import { IconAlbum, IconGauge, IconSettings, IconTransform } from '@tabler/icons-react'
import Image from 'next/image'
import { usePathname } from 'next/navigation'
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
  const router = usePathname()
  const fragment = useFragment<SidebarQuery$key>(FRAGMENT, $key)

  const route = router.split('/')[1]

  return (
    <Navigation isBordered orientation="vertical" size="lg" {...rest}>
      <Navigation.Brand>
        <Image priority alt="giantnodes logo" height={40} src="/images/giantnodes-logo.png" width={128} />
      </Navigation.Brand>

      <Navigation.Segment>
        <Navigation.Item isSelected={route === ''}>
          <Navigation.Link href="/">
            <IconGauge size={20} /> Dashboard
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item isSelected={route === 'recipes'}>
          <Navigation.Link href="/recipes">
            <IconTransform size={20} /> Recipes
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>

      <Navigation.Segment>
        <Navigation.Title className="flex justify-between items-center">
          Your Libraries
          <Button as={Link} color="brand" href="/new" size="xs">
            <IconAlbum size={16} /> New
          </Button>
        </Navigation.Title>

        <Suspense fallback="LOADING...">
          <SidebarLibrarySegment $key={fragment} />
        </Suspense>
      </Navigation.Segment>

      <Navigation.Segment className="mt-auto">
        <Navigation.Item isSelected={route === 'settings'}>
          <Navigation.Link href="/settings/general">
            <IconSettings size={20} /> Settings
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Sidebar
