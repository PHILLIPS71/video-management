import { Avatar, Badge, Navbar } from '@giantnodes/design-system'
import { IconAward } from '@tabler/icons-react'
import Image from 'next/image'

const HomePage = () => (
  <div>
    <Navbar size="md" blurred>
      <Navbar.Brand>
        <Image src="/images/giantnodes-logo.png" alt="giantnodes logo" height={40} width={128} priority />
      </Navbar.Brand>

      <Navbar.Content>
        <Navbar.Item>
          <Badge color="success">Giantnodes</Badge>
        </Navbar.Item>
        <Navbar.Item>Home</Navbar.Item>
        <Navbar.Item>Home</Navbar.Item>
        <Navbar.Item>Home</Navbar.Item>
      </Navbar.Content>

      <Navbar.Content>
        <Navbar.Item>Login</Navbar.Item>
        <Navbar.Item>Logout</Navbar.Item>
      </Navbar.Content>
    </Navbar>

    <Avatar.Group size="md" bordered>
      <Avatar color="neutral">
        <Avatar.Icon icon={<IconAward />} zoomed />
      </Avatar>
      <Avatar color="primary">
        <Avatar.Image src="https://placehold.co/600x400" alt="a placeholder image" zoomed />
      </Avatar>
      <Avatar color="secondary">
        <Avatar.Image src="https://placehold.co/600x400" alt="a placeholder image" zoomed />
      </Avatar>
      <Avatar size="sm" color="success" bordered>
        <Avatar.Image src="https://placehold.co/600x400" alt="a placeholder image" zoomed />
      </Avatar>
      <Avatar color="danger">
        <Avatar.Image src="https://i.pravatar.cc/150?u=a04258114e29026702d" alt="a placeholder image" zoomed />
      </Avatar>
    </Avatar.Group>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>

    <p>
      Maecenas nec enim massa. Donec iaculis eros leo, ac rhoncus enim accumsan a. Donec ac sagittis lorem, non molestie
      arcu. Morbi in suscipit est. Praesent rhoncus elementum massa, laoreet dapibus nisi iaculis sed. Integer dictum
      accumsan lectus. Maecenas at neque vestibulum, hendrerit enim sit amet, varius odio. Donec a arcu ut erat eleifend
      dictum non non tortor. Cras eros felis, laoreet vel mauris non, porttitor venenatis orci. Aliquam dolor justo,
      mattis non ex vitae, congue laoreet lorem. Proin ut dolor congue, efficitur massa et, varius tortor. Pellentesque
      sem odio, venenatis sed orci eu, tempus tristique justo. Curabitur eget dolor nibh. Aliquam ut nunc in eros
      efficitur faucibus eget non mauris.
    </p>
  </div>
)

export default HomePage
