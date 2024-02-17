'use client'

import { Navigation } from '@giantnodes/react'
import { IconHomeCog } from '@tabler/icons-react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'

const SettingSidebar: React.FC = () => {
  const router = usePathname()
  const { library } = useLibraryContext()

  const route = router.split('/')[4]

  return (
    <Navigation orientation="vertical" size="md">
      <Navigation.Segment>
        <Navigation.Title>Settings</Navigation.Title>
      </Navigation.Segment>

      <Navigation.Segment>
        <Navigation.Item>
          <Link legacyBehavior passHref href={`/library/${library.slug}/settings/general`}>
            <Navigation.Link isSelected={route === 'general'}>
              <IconHomeCog size={20} />
              General
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default SettingSidebar
