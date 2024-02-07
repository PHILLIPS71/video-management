'use client'

import type { NavigationProps } from '@giantnodes/react'

import { Navigation } from '@giantnodes/react'
import { IconFolders, IconGauge, IconHome, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'

const Sidebar: React.FC<NavigationProps> = ({ ...rest }) => {
  const { library } = useLibraryContext()
  const router = usePathname()

  const route = router.split('/')[3]

  return (
    <Navigation orientation="vertical" size="sm" {...rest}>
      <Navigation.Brand>
        <Image priority alt="giantnodes logo" height={40} src="/images/giantnodes-logo.png" width={128} />
      </Navigation.Brand>

      <Navigation.Segment>
        <Navigation.Item>
          <Link legacyBehavior passHref href="/">
            <Navigation.Link>
              <IconHome strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Divider />

        <Navigation.Item>
          <Link legacyBehavior passHref href={`/library/${library.slug}`}>
            <Navigation.Link isSelected={route === undefined}>
              <IconGauge strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Item>
          <Link legacyBehavior passHref href={`/library/${library.slug}/explore`}>
            <Navigation.Link isSelected={route === 'explore'}>
              <IconFolders strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>

      <Navigation.Segment className="mt-auto">
        <Navigation.Item>
          <Link legacyBehavior passHref href={`/library/${library.slug}/settings/general`}>
            <Navigation.Link isSelected={route === 'settings'}>
              <IconSettings strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Sidebar
