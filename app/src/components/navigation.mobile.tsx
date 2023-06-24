import type { NavbarProps } from '@giantnodes/design-system-react'

import { Input, Navigation } from '@giantnodes/design-system-react'
import { IconBell, IconSearch } from '@tabler/icons-react'

export type NavigationMobileProps = NavbarProps

const NavigationMobile: React.FC<NavigationMobileProps> = (props) => (
  <Navigation orientation="horizontal" {...props}>
    <Input.Control className="h-full">
      <Input.Addon>
        <IconSearch size={20} />
      </Input.Addon>
      <Input className="h-full" placeholder="Search..." type="text" />
    </Input.Control>

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
