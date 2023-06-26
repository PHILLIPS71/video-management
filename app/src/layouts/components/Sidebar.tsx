import type { NavigationProps } from '@giantnodes/design-system-react'

import { Navigation } from '@giantnodes/design-system-react'
import { IconAlertCircle, IconFolders, IconGauge, IconSettings } from '@tabler/icons-react'
import Image from 'next/image'
import Link from 'next/link'

export type SidebarProps = NavigationProps

const Sidebar: React.FC<SidebarProps> = (props) => (
  <Navigation orientation="vertical" {...props}>
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

export default Sidebar
