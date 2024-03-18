import type { EncodeProfileTable_DeleteEncodeProfileMutation } from '@/__generated__/EncodeProfileTable_DeleteEncodeProfileMutation.graphql'
import type {
  EncodeProfileTableFragment$data,
  EncodeProfileTableFragment$key,
} from '@/__generated__/EncodeProfileTableFragment.graphql'
import type { EncodeProfileTableRefetchQuery } from '@/__generated__/EncodeProfileTableRefetchQuery.graphql'
import type { EncodeProfileUpdateInput } from '@/components/interfaces/profiles'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconAlertTriangle, IconEdit, IconTrash } from '@tabler/icons-react'
import React from 'react'
import { graphql, useMutation, usePaginationFragment } from 'react-relay'

import { EncodeProfileDialog } from '@/components/interfaces/profiles'

const QUERY = graphql`
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
          use_hardware_acceleration
          is_encodable
          container {
            id
          }
          codec {
            id
            name
          }
          preset {
            id
            name
          }
          tune {
            id
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

const MUTATION = graphql`
  mutation EncodeProfileTable_DeleteEncodeProfileMutation($input: Encode_profile_deleteInput!) {
    encode_profile_delete(input: $input) {
      encodeProfile {
        id @deleteRecord
      }
      errors {
        ... on DomainError {
          message
        }
        ... on ValidationError {
          message
        }
      }
    }
  }
`

type EncodeProfileTableProps = {
  $key: EncodeProfileTableFragment$key
}

type EncodeProfileNode = NonNullable<
  NonNullable<EncodeProfileTableFragment$data['encode_profiles']>['edges']
>[0]['node']

const EncodeProfileTable: React.FC<EncodeProfileTableProps> = ({ $key }) => {
  const { data } = usePaginationFragment<EncodeProfileTableRefetchQuery, EncodeProfileTableFragment$key>(QUERY, $key)

  const [commit] = useMutation<EncodeProfileTable_DeleteEncodeProfileMutation>(MUTATION)

  const getProfileInput = (node: EncodeProfileNode): EncodeProfileUpdateInput => ({
    id: node.id,
    name: node.name,
    container: node.container?.id,
    codec: node.codec.id,
    preset: node.preset.id,
    tune: node.tune?.id,
    quality: node.quality,
    use_hardware_acceleration: node.use_hardware_acceleration,
  })

  const remove = React.useCallback(
    (entry: EncodeProfileNode) => {
      commit({
        variables: {
          input: {
            id: entry.id,
          },
        },
      })
    },
    [commit]
  )

  return (
    <Table aria-label="encode profile table">
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
                <EncodeProfileDialog profile={getProfileInput(item.node)}>
                  <Button color="neutral" size="xs">
                    <IconEdit size={16} />
                  </Button>
                </EncodeProfileDialog>

                <Button color="danger" size="xs" onPress={() => remove(item.node)}>
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
