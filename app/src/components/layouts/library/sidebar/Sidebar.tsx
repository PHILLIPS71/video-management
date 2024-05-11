'use client'

import type { NavigationProps } from '@giantnodes/react'

import { Navigation } from '@giantnodes/react'
import { IconFolders, IconGauge, IconHome, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import { usePathname } from 'next/navigation'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'

const Sidebar: React.FC<NavigationProps> = ({ ...rest }) => {
  const { library } = useLibraryContext()
  const router = usePathname()

  const route = router.split('/')[3]

  return (
    <Navigation isBordered orientation="vertical" size="sm" {...rest}>
      <Navigation.Brand>
        <Image priority alt="giantnodes logo" height={40} src="/images/giantnodes-logo.png" width={128} />
      </Navigation.Brand>

      <Navigation.Segment>
        <Navigation.Item>
          <Navigation.Link href="/">
            <IconHome href="/" strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Divider />

        <Navigation.Item isSelected={route === undefined}>
          <Navigation.Link href={`/library/${library.slug}`}>
            <IconGauge strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item isSelected={route === 'explore'}>
          <Navigation.Link href={`/library/${library.slug}/explore`}>
            <IconFolders strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>

      <Navigation.Segment className="mt-auto">
        <Navigation.Item isSelected={route === 'settings'}>
          <Navigation.Link href={`/library/${library.slug}/settings/general`}>
            <IconSettings strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Sidebar
