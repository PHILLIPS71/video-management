'use client'

import type { NavigationProps } from '@giantnodes/react'

import { Navigation } from '@giantnodes/react'
import { IconFolders, IconGauge, IconHome, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.context'

const Sidebar: React.FC<NavigationProps> = ({ ...rest }) => {
  const { library } = useLibraryContext()

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
            <Navigation.Link>
              <IconGauge strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Item>
          <Link legacyBehavior passHref href={`/library/${library.slug}/explore`}>
            <Navigation.Link>
              <IconFolders strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>

      <Navigation.Segment className="mt-auto">
        <Navigation.Item>
          <Link legacyBehavior passHref href="/">
            <Navigation.Link>
              <IconSettings strokeWidth={1.5} />
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Sidebar
