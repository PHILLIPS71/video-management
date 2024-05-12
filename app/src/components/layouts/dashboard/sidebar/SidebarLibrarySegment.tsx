'use client'

import type { SidebarLibrarySegmentFragment$key } from '@/__generated__/SidebarLibrarySegmentFragment.graphql'
import type {
  FileSystemStatus,
  SidebarLibrarySegmentRefetchQuery,
} from '@/__generated__/SidebarLibrarySegmentRefetchQuery.graphql'
import type { AvatarProps } from '@giantnodes/react'

import { Avatar, Navigation } from '@giantnodes/react'
import { IconFolderCheck, IconFolderExclamation, IconFolderQuestion, IconFolderX } from '@tabler/icons-react'
import { graphql, usePaginationFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment SidebarLibrarySegmentFragment on Query
  @refetchable(queryName: "SidebarLibrarySegmentRefetchQuery")
  @argumentDefinitions(
    first: { type: "Int" }
    after: { type: "String" }
    where: { type: "LibraryFilterInput" }
    order: { type: "[LibrarySortInput!]" }
  ) {
    libraries(first: $first, after: $after, where: $where, order: $order)
      @connection(key: "SidebarLibrarySegmentFragment_libraries") {
      edges {
        node {
          id
          name
          slug
          status
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

const SidebarLibrarySegment: React.FC<SidebarLibrarySegmentProps> = ({ $key }) => {
  const { data, loadNext, hasNext } = usePaginationFragment<
    SidebarLibrarySegmentRefetchQuery,
    SidebarLibrarySegmentFragment$key
  >(FRAGMENT, $key)

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

  const getDriveStatusColor = (status: FileSystemStatus): AvatarProps['color'] => {
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
    <>
      {data?.libraries?.edges?.map((edge) => (
        <Navigation.Item key={edge.node.id}>
          <Navigation.Link href={`/library/${edge.node.slug}/explore`}>
            <Avatar radius="md" size="sm">
              <Avatar.Notification color={getDriveStatusColor(edge.node.status)} />
              <Avatar.Icon icon={getDriveStatusIcon(edge.node.status)} />
            </Avatar>

            {edge.node.name}
          </Navigation.Link>
        </Navigation.Item>
      ))}

      {hasNext && <Navigation.Title onClick={() => loadNext(10)}>Show more</Navigation.Title>}
    </>
  )
}

export default SidebarLibrarySegment
