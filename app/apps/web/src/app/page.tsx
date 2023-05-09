'use client'

import { Avatar } from '@giantnodes/design-system'
import React from 'react'

import { IconAward } from '@tabler/icons-react'

const HomePage = () => (
  <div>
    Home Page
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
  </div>
)

export default HomePage
