import type { ExploreTableFragment$key } from '@/__generated__/ExploreTableFragment.graphql'

import { Input, Table, Typography } from '@giantnodes/react'
import { IconSearch } from '@tabler/icons-react'
import dayjs from 'dayjs'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import ExploreTableDirectory from '@/components/interfaces/explore/table/ExploreTableDirectory'
import ExploreTableFile from '@/components/interfaces/explore/table/ExploreTableFile'
import { useExploreContext } from '@/components/interfaces/explore/use-explore.hook'

const FRAGMENT = graphql`
  fragment ExploreTableFragment on FileSystemDirectory
  @argumentDefinitions(order: { type: "[FileSystemEntrySortInput!]" }) {
    scanned_at
    entries(order: $order) {
      __typename
      id
      size
      ... on FileSystemDirectory {
        ...ExploreTableDirectoryFragment
      }
      ... on FileSystemFile {
        ...ExploreTableFileFragment
      }
    }
  }
`

type ExploreTableProps = {
  $key: ExploreTableFragment$key
}

const ExploreTable: React.FC<ExploreTableProps> = ({ $key }) => {
  const { keys, setKeys } = useExploreContext()

  const data = useFragment(FRAGMENT, $key)

  return (
    <Table
      aria-label="explore table"
      mode="multiple"
      selectedKeys={keys}
      onSelectionChange={(selection) => setKeys(selection)}
    >
      <Table.Head>
        <Table.Column key="name" isRowHeader>
          <Input.Group aria-label="search file system entries" className="max-w-xs">
            <Input.Addon>
              <IconSearch size={16} />
            </Input.Addon>
            <Input aria-label="search library" placeholder="Search by name" type="text" />
          </Input.Group>
        </Table.Column>
        <Table.Column key="size" className="text-right">
          <Typography.Text variant="subtitle">{dayjs(data.scanned_at).fromNow()}</Typography.Text>
        </Table.Column>
      </Table.Head>

      <Table.Body items={data.entries}>
        {(item) => (
          <Table.Row id={item.id}>
            <Table.Cell>
              <div className="flex flex-row items-center gap-2">
                {item.__typename === 'FileSystemDirectory' && <ExploreTableDirectory $key={item} />}

                {item.__typename === 'FileSystemFile' && <ExploreTableFile $key={item} />}
              </div>
            </Table.Cell>

            <Table.Cell>
              <Typography.Paragraph className="text-right">{filesize(item.size, { base: 2 })}</Typography.Paragraph>
            </Table.Cell>
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  )
}

export default ExploreTable
