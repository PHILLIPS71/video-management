'use client'

import { Navigation } from '@giantnodes/react'
import { IconHomeCog, IconServerCog, IconUserCog } from '@tabler/icons-react'
import { usePathname } from 'next/navigation'

const SettingSidebar: React.FC = () => {
  const router = usePathname()

  const route = router.split('/')[2]

  return (
    <Navigation isBordered orientation="vertical" size="md">
      <Navigation.Segment>
        <Navigation.Title>Settings</Navigation.Title>
      </Navigation.Segment>

      <Navigation.Segment>
        <Navigation.Item isSelected={route === 'general'}>
          <Navigation.Link href="/settings/general">
            <IconHomeCog size={20} /> General
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item isSelected={route === 'preferences'}>
          <Navigation.Link href="/settings/preferences">
            <IconUserCog size={20} />
            Preferences
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item isSelected={route === 'encoder'}>
          <Navigation.Link href="/settings/encoder">
            <IconServerCog size={20} />
            Encoder
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default SettingSidebar
