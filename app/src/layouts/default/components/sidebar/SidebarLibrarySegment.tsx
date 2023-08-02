'use client'

import type { SidebarLibrarySegmentFragment$key } from '@/__generated__/SidebarLibrarySegmentFragment.graphql'
import type { SidebarLibrarySegmentPaginationQuery } from '@/__generated__/SidebarLibrarySegmentPaginationQuery.graphql'

import { Button, Navigation } from '@giantnodes/design-system-react'
import { IconAlbum } from '@tabler/icons-react'
import Link from 'next/link'
import { graphql, usePaginationFragment } from 'react-relay'

import SidebarLibrarySegmentItem from '@/layouts/default/components/sidebar/SidebarLibrarySegmentItem'

const SidebarLibrarySegmentFragment = graphql`
  fragment SidebarLibrarySegmentFragment on Query
  @refetchable(queryName: "SidebarLibrarySegmentPaginationQuery")
  @argumentDefinitions(first: { type: "Int" }, after: { type: "String" }, order: { type: "[LibrarySortInput!]" }) {
    libraries(first: $first, after: $after, order: $order)
      @connection(key: "SidebarLibrarySegmentFragment_libraries", filters: []) {
      edges {
        node {
          ...SidebarLibrarySegmentItemFragment
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
    SidebarLibrarySegmentPaginationQuery,
    SidebarLibrarySegmentFragment$key
  >(SidebarLibrarySegmentFragment, $key)

  return (
    <Navigation.Segment>
      <Navigation.Title className="flex justify-between items-center">
        Your Libraries
        <Link passHref href="/new">
          <Button color="primary" size="xs">
            <IconAlbum size={16} /> New
          </Button>
        </Link>
      </Navigation.Title>

      {data?.libraries?.edges?.map((edge, index) => <SidebarLibrarySegmentItem key={index} $key={edge.node} />)}

      {hasNext && <Navigation.Title onClick={() => loadNext(10)}>Show more</Navigation.Title>}
    </Navigation.Segment>
  )
}

export default SidebarLibrarySegment
