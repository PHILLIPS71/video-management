import type { EncodeProfileTableFragment$key } from '@/__generated__/EncodeProfileTableFragment.graphql'
import type { EncodeProfileTableRefetchQuery } from '@/__generated__/EncodeProfileTableRefetchQuery.graphql'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconAlertTriangle, IconEdit, IconTrash } from '@tabler/icons-react'
import React from 'react'
import { graphql, usePaginationFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodeProfileTableFragment on Query
  @refetchable(queryName: "EncodeProfileTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "EncodeProfileFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeProfileSortInput!]" }
  ) {
    encode_profiles(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodeProfileTableFragment_encode_profiles", filters: []) {
      edges {
        node {
          id
          name
          quality
          is_encodable
          codec {
            name
          }
          preset {
            name
          }
          tune {
            name
          }
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

type EncodeProfileTableProps = {
  $key: EncodeProfileTableFragment$key
}

const EncodeProfileTable: React.FC<EncodeProfileTableProps> = ({ $key }) => {
  const { data } = usePaginationFragment<EncodeProfileTableRefetchQuery, EncodeProfileTableFragment$key>(FRAGMENT, $key)

  return (
    <Table aria-label="encode profile table" size="sm">
      <Table.Head>
        <Table.Column key="name" isRowHeader>
          Name
        </Table.Column>
        <Table.Column key="codec">Codec</Table.Column>
        <Table.Column key="preset">Preset</Table.Column>
        <Table.Column key="quality">Quality</Table.Column>
        <Table.Column key="tune">Tune</Table.Column>
        <Table.Column key="controls" />
      </Table.Head>

      <Table.Body items={data.encode_profiles?.edges ?? []}>
        {(item) => (
          <Table.Row id={item.node.id}>
            <Table.Cell>
              <div className="flex flex-row  gap-3">
                <Typography.Paragraph>{item.node.name}</Typography.Paragraph>

                {!item.node.is_encodable && (
                  <Chip color="danger">
                    <IconAlertTriangle size={14} />
                  </Chip>
                )}
              </div>
            </Table.Cell>
            <Table.Cell>
              <Chip color="success">{item.node.codec.name.toLocaleLowerCase()}</Chip>
            </Table.Cell>
            <Table.Cell>
              <Chip color="warning">{item.node.preset.name.toLocaleLowerCase()}</Chip>
            </Table.Cell>
            <Table.Cell>{item.node.quality != null && <Chip color="info">{item.node.quality}</Chip>}</Table.Cell>
            <Table.Cell>
              {item.node.tune && <Chip color="info">{item.node.tune?.name?.toLocaleLowerCase()}</Chip>}
            </Table.Cell>
            <Table.Cell>
              <div className="flex flex-row justify-end gap-3">
                <Button color="neutral" size="xs">
                  <IconEdit size={16} />
                </Button>

                <Button color="danger" size="xs">
                  <IconTrash size={16} />
                </Button>
              </div>
            </Table.Cell>
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  )
}

export default EncodeProfileTable
