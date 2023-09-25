'use client'

import type {
  FileSystemStatus,
  SidebarLibrarySegmentItemFragment$key,
} from '@/__generated__/SidebarLibrarySegmentItemFragment.graphql'
import type { AvatarVariantProps } from '@giantnodes/react'

import { Avatar, Navigation } from '@giantnodes/react'
import { IconFolderCheck, IconFolderExclamation, IconFolderQuestion, IconFolderX } from '@tabler/icons-react'
import Link from 'next/link'
import { graphql, useFragment } from 'react-relay'

type SidebarLibrarySegmentItemProps = {
  $key: SidebarLibrarySegmentItemFragment$key
}

const SidebarLibrarySegmentItem: React.FC<SidebarLibrarySegmentItemProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment SidebarLibrarySegmentItemFragment on Library {
        id
        name
        slug
        status
      }
    `,
    $key
  )

  const getDriveStatusIcon = (status: FileSystemStatus) => {
    switch (status) {
      case 'ONLINE':
        return <IconFolderCheck size={14} />
      case 'DEGRADED':
        return <IconFolderExclamation size={14} />
      case 'OFFLINE':
        return <IconFolderX size={14} />
      default:
        return <IconFolderQuestion size={14} />
    }
  }

  const getDriveStatusColor = (status: FileSystemStatus): AvatarVariantProps['status'] => {
    switch (status) {
      case 'ONLINE':
        return 'success'
      case 'DEGRADED':
        return 'warning'
      case 'OFFLINE':
        return 'danger'
      default:
        return 'neutral'
    }
  }

  return (
    <Navigation.Item>
      <Link legacyBehavior passHref href={`/library/${data.slug}`}>
        <Navigation.Link>
          <Avatar radius="md" size="xs">
            <Avatar.Notification color={getDriveStatusColor(data.status)} />
            <Avatar.Icon icon={getDriveStatusIcon(data.status)} />
          </Avatar>

          {data.name}
        </Navigation.Link>
      </Link>
    </Navigation.Item>
  )
}

export default SidebarLibrarySegmentItem
