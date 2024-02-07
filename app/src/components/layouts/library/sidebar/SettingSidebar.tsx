'use client'

import { Navigation } from '@giantnodes/react'
import { usePathname } from 'next/navigation'

const SettingSidebar: React.FC = () => {
  const router = usePathname()

  const route = router.split('/')[4]

  return (
    <Navigation orientation="vertical" size="md">
      <Navigation.Segment>
        <Navigation.Title>Settings</Navigation.Title>
      </Navigation.Segment>

      <Navigation.Segment>
        <Navigation.Item>
          <Navigation.Link isSelected={route === 'general'}>General</Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default SettingSidebar
