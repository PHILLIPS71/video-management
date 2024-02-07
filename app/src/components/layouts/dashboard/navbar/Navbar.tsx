import type { NavigationProps } from '@giantnodes/react'

import { Input, Navigation } from '@giantnodes/react'
import { IconBell, IconSearch } from '@tabler/icons-react'

const Navbar: React.FC<NavigationProps> = (props) => (
  <Navigation orientation="horizontal" {...props}>
    <Navigation.Segment className="grow">
      <Navigation.Item>
        <Input>
          <Input.Addon>
            <IconSearch size={20} />
          </Input.Addon>
          <Input.Control aria-label="search" placeholder="Search..." type="text" />
        </Input>
      </Navigation.Item>
    </Navigation.Segment>

    <Navigation.Segment>
      <Navigation.Item>
        <Navigation.Trigger>
          <IconBell strokeWidth={1.5} />
        </Navigation.Trigger>
      </Navigation.Item>
    </Navigation.Segment>
  </Navigation>
)

export default Navbar
