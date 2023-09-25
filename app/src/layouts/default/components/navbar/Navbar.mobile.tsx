'use client'

import type { NavbarProps } from '@giantnodes/react'

import { Input, Navigation } from '@giantnodes/react'
import { IconBell, IconSearch } from '@tabler/icons-react'

export type NavigationMobileProps = NavbarProps

const NavigationMobile: React.FC<NavigationMobileProps> = (props) => (
  <Navigation orientation="horizontal" {...props}>
    <Input variant="none">
      <Input.Addon>
        <IconSearch size={20} />
      </Input.Addon>
      <Input.Control placeholder="Search..." type="text" />
    </Input>

    <Navigation.Segment className="ml-auto">
      <Navigation.Item>
        <Navigation.Link>
          <IconBell strokeWidth={1.5} />
        </Navigation.Link>
      </Navigation.Item>
    </Navigation.Segment>
  </Navigation>
)

export default NavigationMobile
