'use client'

import type {
  DriveStatus,
  SidebarLibrarySegmentFragment$key,
} from '@/__generated__/SidebarLibrarySegmentFragment.graphql'
import type { SidebarLibrarySegmentPaginationQuery } from '@/__generated__/SidebarLibrarySegmentPaginationQuery.graphql'
import type { AvatarVariantProps } from '@giantnodes/design-system-react'

import { Avatar, Navigation } from '@giantnodes/design-system-react'
import { IconFolderCheck, IconFolderExclamation, IconFolderQuestion, IconFolderX } from '@tabler/icons-react'
import Link from 'next/link'
import { graphql, usePaginationFragment } from 'react-relay'

export const SidebarLibrarySegmentFragment = graphql`
  fragment SidebarLibrarySegmentFragment on Query
  @refetchable(queryName: "SidebarLibrarySegmentPaginationQuery")
  @argumentDefinitions(cursor: { type: "String" }, count: { type: "Int" }) {
    libraries(after: $cursor, first: $count) @connection(key: "SidebarLibrarySegment_libraries") {
      edges {
        node {
          id
          name
          drive_status
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

type SidebarLibrarySegmentProps = {
  $key: SidebarLibrarySegmentFragment$key
}

const SidebarLibrarySegment = ({ $key }: SidebarLibrarySegmentProps) => {
  const { data, loadNext, hasNext } = usePaginationFragment<
    SidebarLibrarySegmentPaginationQuery,
    SidebarLibrarySegmentFragment$key
  >(SidebarLibrarySegmentFragment, $key)

  const getDriveStatusIcon = (status: DriveStatus) => {
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

  const getDriveStatusColor = (status: DriveStatus): AvatarVariantProps['status'] => {
    switch (status) {
      case 'ONLINE':
        return 'green'
      case 'DEGRADED':
        return 'yellow'
      case 'OFFLINE':
        return 'red'
      default:
        return 'gray'
    }
  }

  return (
    <Navigation.Segment>
      <Navigation.Title>Your Libraries</Navigation.Title>

      {data?.libraries?.edges?.map((edge) => (
        <Navigation.Item key={edge.node.id}>
          <Link legacyBehavior passHref href="/">
            <Navigation.Link>
              <Avatar radius="md" size="xs">
                <Avatar.Notification status={getDriveStatusColor(edge.node.drive_status)} />
                <Avatar.Icon icon={getDriveStatusIcon(edge.node.drive_status)} />
              </Avatar>

              {edge.node.name}
            </Navigation.Link>
          </Link>
        </Navigation.Item>
      ))}

      {hasNext && <Navigation.Title onClick={() => loadNext(10)}>Show more</Navigation.Title>}
    </Navigation.Segment>
  )
}

export default SidebarLibrarySegment
