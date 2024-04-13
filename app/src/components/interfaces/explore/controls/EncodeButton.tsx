import type { EncodeButton_EncodeSubmitMutation } from '@/__generated__/EncodeButton_EncodeSubmitMutation.graphql'
import type { EncodeButtonFragment$key } from '@/__generated__/EncodeButtonFragment.graphql'
import type { EncodeButtonQuery } from '@/__generated__/EncodeButtonQuery.graphql'
import type { Selection } from '@giantnodes/react'

import { Button, Chip, Menu } from '@giantnodes/react'
import { IconCaretDownFilled } from '@tabler/icons-react'
import React from 'react'
import { graphql, useLazyLoadQuery, useMutation, usePaginationFragment } from 'react-relay'

import { useExploreContext } from '@/components/interfaces/explore/use-explore.hook'

const QUERY = graphql`
  query EncodeButtonQuery($first: Int, $after: String, $order: [RecipeSortInput!]) {
    ...EncodeButtonFragment @arguments(first: $first, after: $after, order: $order)
  }
`

const FRAGMENT = graphql`
  fragment EncodeButtonFragment on Query
  @refetchable(queryName: "EncodeButtonRefetchQuery")
  @argumentDefinitions(first: { type: "Int" }, after: { type: "String" }, order: { type: "[RecipeSortInput!]" }) {
    recipes(first: $first, after: $after, order: $order) @connection(key: "EncodeButtonFragment_recipes") {
      edges {
        node {
          id
          name
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

const MUTATION = graphql`
  mutation EncodeButton_EncodeSubmitMutation($input: Encode_submitInput!) {
    encode_submit(input: $input) {
      encode {
        id
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

const EncodeButton: React.FC = () => {
  const { directory, keys, setErrors } = useExploreContext()

  const query = useLazyLoadQuery<EncodeButtonQuery>(QUERY, {
    first: 8,
    order: [{ name: 'ASC' }],
  })

  const { data, hasNext, loadNext } = usePaginationFragment<EncodeButtonQuery, EncodeButtonFragment$key>(
    FRAGMENT,
    query
  )

  const [commit, isLoading] = useMutation<EncodeButton_EncodeSubmitMutation>(MUTATION)

  const isDisabled = React.useMemo<boolean>(() => {
    if (typeof keys === 'string') return false

    if (typeof keys === 'object') return keys.size === 0

    return true
  }, [keys])

  const entries = React.useMemo<string[]>(() => {
    if (typeof keys === 'string') return [directory]

    if (typeof keys === 'object') return Array.from(keys.values()).map((key) => key.toString())

    return []
  }, [directory, keys])

  const disabledKeys = React.useMemo<string[]>(
    () => data.recipes?.edges?.filter((x) => !x.node.is_encodable).map((x) => x.node.id) ?? [],
    [data.recipes]
  )

  const onPress = (key: Selection) => {
    commit({
      variables: {
        input: {
          recipe_id: key,
          entries,
        },
      },
      onCompleted: (payload) => {
        if (payload.encode_submit.errors != null) {
          const faults = payload.encode_submit.errors
            .filter((error) => error.message !== undefined)
            .map((error) => error.message!)

          setErrors(faults)

          return
        }

        setErrors([])
      },
      onError: (error) => {
        setErrors([error.message])
      },
    })
  }

  return (
    <Menu size="xs">
      <Button className="flex items-center flex-row" color="brand" isDisabled={isDisabled || isLoading} size="xs">
        <span>Encode</span>
        <IconCaretDownFilled size={16} />
      </Button>

      <Menu.Popover placement="bottom right">
        <Menu.List disabledKeys={disabledKeys} onAction={(key) => onPress(key)}>
          {data.recipes?.edges?.map((recipe) => (
            <Menu.Item key={recipe.node.id} className="flex items-center gap-2" id={recipe.node.id}>
              {recipe.node.name}

              <div className="flex items-end flex-grow justify-end gap-2">
                <Chip color="success" size="sm">
                  {recipe.node.codec.name.toLocaleLowerCase()}
                </Chip>

                <Chip color="info" size="sm">
                  {recipe.node.preset.name.toLocaleLowerCase()}
                </Chip>

                {recipe.node.tune && (
                  <Chip color="warning" size="sm">
                    {recipe.node.tune.name.toLocaleLowerCase()}
                  </Chip>
                )}
              </div>
            </Menu.Item>
          ))}
        </Menu.List>

        {hasNext && (
          <div className="flex flex-row items-center justify-center p-2">
            <Button size="xs" onPress={() => loadNext(8)}>
              Show more
            </Button>
          </div>
        )}
      </Menu.Popover>
    </Menu>
  )
}

export default EncodeButton
