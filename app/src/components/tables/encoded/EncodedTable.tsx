import type { EncodedTableFragment$key } from '@/__generated__/EncodedTableFragment.graphql'
import type { EncodedTableRefetchQuery } from '@/__generated__/EncodedTableRefetchQuery.graphql'

import { Button, Link, Table } from '@giantnodes/react'
import React from 'react'
import { graphql, usePaginationFragment } from 'react-relay'

import EncodeDialog from '@/components/interfaces/dashboard/EncodeDialog'
import { EncodeBadges } from '@/components/ui'

const FRAGMENT = graphql`
  fragment EncodedTableFragment on Query
  @refetchable(queryName: "EncodedTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "EncodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeSortInput!]" }
  ) {
    complete: encodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodedTableFragment_complete", filters: []) {
      edges {
        node {
          id
          file {
            path_info {
              name
            }
          }
          ...EncodeBadgesFragment
          ...EncodeDialogFragment
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

type EncodedTableProps = {
  $key: EncodedTableFragment$key
}

const EncodedTable: React.FC<EncodedTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<EncodedTableRefetchQuery, EncodedTableFragment$key>(
    FRAGMENT,
    $key
  )

  return (
    <>
      <Table headingless aria-label="encode table">
        <Table.Head>
          <Table.Column key="name" isRowHeader>
            name
          </Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>

        <Table.Body items={data.complete?.edges ?? []}>
          {(item) => (
            <Table.Row id={item.node.id}>
              <Table.Cell>
                <EncodeDialog $key={item.node}>
                  <Button as={Link} color="transparent">
                    {item.node.file.path_info.name}
                  </Button>
                </EncodeDialog>
              </Table.Cell>
              <Table.Cell>
                <EncodeBadges $key={item.node} />
              </Table.Cell>
            </Table.Row>
          )}
        </Table.Body>
      </Table>

      {hasNext && (
        <div className="flex flex-row items-center justify-center p-2">
          <Button size="xs" onPress={() => loadNext(8)}>
            Show more
          </Button>
        </div>
      )}
    </>
  )
}

export default EncodedTable
