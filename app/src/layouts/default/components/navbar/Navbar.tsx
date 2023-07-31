'use client'

import type { NavigationProps } from '@giantnodes/design-system-react'

import { Input, Navigation } from '@giantnodes/design-system-react'
import { IconBell, IconSearch } from '@tabler/icons-react'

export type NavbarProps = NavigationProps

const Navbar: React.FC<NavbarProps> = (props) => (
  <Navigation orientation="horizontal" {...props}>
    <Input variant="none">
      <Input.Addon>
        <IconSearch size={20} />
      </Input.Addon>
      <Input.Control placeholder="Search..." type="text" />
    </Input>

    <Navigation.Segment className="ml-auto">
      <Navigation.Item>
        <Navigation.Trigger>
          <IconBell strokeWidth={1.5} />
        </Navigation.Trigger>
      </Navigation.Item>
    </Navigation.Segment>
  </Navigation>
)

export default Navbar
