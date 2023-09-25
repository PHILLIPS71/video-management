'use client'

import type { NavigationProps } from '@giantnodes/react'

import { Breadcrumb, Input, Navigation } from '@giantnodes/react'
import { IconBell, IconSearch } from '@tabler/icons-react'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.context'

export type NavbarProps = NavigationProps

const Navbar: React.FC<NavbarProps> = (props) => {
  const { library } = useLibraryContext()

  return (
    <Navigation orientation="horizontal" {...props}>
      <Navigation.Segment>
        <Navigation.Item>
          <Breadcrumb>
            <Breadcrumb.Item>Library</Breadcrumb.Item>
            <Breadcrumb.Item>{library.name}</Breadcrumb.Item>
          </Breadcrumb>
        </Navigation.Item>

        <Navigation.Divider />

        <Navigation.Item>
          <Input variant="none">
            <Input.Addon>
              <IconSearch size={20} />
            </Input.Addon>
            <Input.Control placeholder="Search..." type="text" />
          </Input>
        </Navigation.Item>
      </Navigation.Segment>

      <Navigation.Segment className="ml-auto">
        <Navigation.Item>
          <Navigation.Trigger>
            <IconBell strokeWidth={1.5} />
          </Navigation.Trigger>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default Navbar
