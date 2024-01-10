import type { ExploreTableFragment$data, ExploreTableFragment$key } from '@/__generated__/ExploreTableFragment.graphql'

import { Input, Table, Typography } from '@giantnodes/react'
import { IconSearch } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import ExploreTableDirectory from '@/components/explore/ExploreTableDirectory'
import ExploreTableFile from '@/components/explore/ExploreTableFile'

type ExploreTableProps = {
  $key: ExploreTableFragment$key
  onChange?: React.Dispatch<React.SetStateAction<Set<string>>>
}

type ExploreTableEntry = ExploreTableFragment$data['entries'][0]

const ExploreTable: React.FC<ExploreTableProps> = ({ $key, onChange }) => {
  const data = useFragment(
    graphql`
      fragment ExploreTableFragment on FileSystemDirectory
      @argumentDefinitions(order: { type: "[FileSystemEntrySortInput!]" }) {
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
    `,
    $key
  )

  const render = React.useCallback((item: ExploreTableEntry, key: React.Key) => {
    switch (key) {
      case 'name':
        return (
          <div className="flex flex-row items-center gap-2">
            {item.__typename === 'FileSystemDirectory' && <ExploreTableDirectory $key={item} />}

            {item.__typename === 'FileSystemFile' && <ExploreTableFile $key={item} />}
          </div>
        )

      case 'size':
        return <Typography.Text className="text-right">{filesize(item.size, { base: 2 })}</Typography.Text>

      default:
        throw new Error(`the key '${key}' is unexpected`)
    }
  }, [])

  return (
    <Table aria-label="a cool table" mode="multiple" onSelectionChange={onChange}>
      <Table.Head>
        <Table.Column key="name" className="py-2 select-all">
          <Input className="max-w-xs">
            <Input.Addon>
              <IconSearch size={16} />
            </Input.Addon>
            <Input.Control placeholder="Search by name" type="text" />
          </Input>
        </Table.Column>
        <Table.Column key="size" className="py-2 select-all">
          <Typography.Text className="text-xs text-right" variant="subtitle">
            4 weeks ago
          </Typography.Text>
        </Table.Column>
      </Table.Head>
      <Table.Body items={data.entries}>
        {(item) => <Table.Row key={item.id}>{(key) => <Table.Cell>{render(item, key)}</Table.Cell>}</Table.Row>}
      </Table.Body>
    </Table>
  )
}

ExploreTable.defaultProps = {
  onChange: undefined,
}

export default ExploreTable
