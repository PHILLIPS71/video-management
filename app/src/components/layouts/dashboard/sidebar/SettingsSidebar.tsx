'use client'

import { Navigation } from '@giantnodes/react'
import { IconHomeCog, IconServerCog, IconUserCog } from '@tabler/icons-react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const SettingSidebar: React.FC = () => {
  const router = usePathname()

  const route = router.split('/')[2]

  return (
    <Navigation orientation="vertical" size="md">
      <Navigation.Segment>
        <Navigation.Title>Settings</Navigation.Title>
      </Navigation.Segment>

      <Navigation.Segment>
        <Navigation.Item>
          <Link legacyBehavior passHref href="/settings/general">
            <Navigation.Link isSelected={route === 'general'}>
              <IconHomeCog size={20} /> General
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Item>
          <Link legacyBehavior passHref href="/settings/preferences">
            <Navigation.Link isSelected={route === 'preferences'}>
              <IconUserCog size={20} />
              Preferences
            </Navigation.Link>
          </Link>
        </Navigation.Item>

        <Navigation.Item>
          <Link legacyBehavior passHref href="/settings/encoder">
            <Navigation.Link isSelected={route === 'encoder'}>
              <IconServerCog size={20} />
              Encoder
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default SettingSidebar
