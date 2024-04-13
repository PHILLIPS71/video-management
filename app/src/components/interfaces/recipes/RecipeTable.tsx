import type { RecipeTable_DeleteRecipeMutation } from '@/__generated__/RecipeTable_DeleteRecipeMutation.graphql'
import type { RecipeTableFragment$data, RecipeTableFragment$key } from '@/__generated__/RecipeTableFragment.graphql'
import type { RecipeTableRefetchQuery } from '@/__generated__/RecipeTableRefetchQuery.graphql'
import type { RecipeUpdateInput } from '@/components/interfaces/recipes'

import { Button, Chip, Table, Typography } from '@giantnodes/react'
import { IconAlertTriangle, IconEdit, IconTrash } from '@tabler/icons-react'
import React from 'react'
import { graphql, useMutation, usePaginationFragment } from 'react-relay'

import { RecipeDialog } from '@/components/interfaces/recipes'

const QUERY = graphql`
  fragment RecipeTableFragment on Query
  @refetchable(queryName: "RecipeTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "RecipeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[RecipeSortInput!]" }
  ) {
    recipes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "RecipeTableFragment_recipes", filters: []) {
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
  mutation RecipeTable_DeleteRecipeMutation($input: Recipe_deleteInput!) {
    recipe_delete(input: $input) {
      recipe {
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

type RecipeTableProps = {
  $key: RecipeTableFragment$key
}

type RecipeNode = NonNullable<NonNullable<RecipeTableFragment$data['recipes']>['edges']>[0]['node']

const RecipeTable: React.FC<RecipeTableProps> = ({ $key }) => {
  const { data } = usePaginationFragment<RecipeTableRefetchQuery, RecipeTableFragment$key>(QUERY, $key)

  const [commit] = useMutation<RecipeTable_DeleteRecipeMutation>(MUTATION)

  const getRecipeInput = (node: RecipeNode): RecipeUpdateInput => ({
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
    (entry: RecipeNode) => {
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
    <Table aria-label="recipe table">
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

      <Table.Body items={data.recipes?.edges ?? []}>
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
                <RecipeDialog recipe={getRecipeInput(item.node)}>
                  <Button color="neutral" size="xs">
                    <IconEdit size={16} />
                  </Button>
                </RecipeDialog>

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

export default RecipeTable
