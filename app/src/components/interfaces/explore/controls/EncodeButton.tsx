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
  query EncodeButtonQuery {
    ...EncodeButtonFragment
  }
`

const FRAGMENT = graphql`
  fragment EncodeButtonFragment on Query
  @refetchable(queryName: "EncodeButtonRefetchQuery")
  @argumentDefinitions(
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeProfileSortInput!]" }
  ) {
    encode_profiles(first: $first, after: $after, order: $order)
      @connection(key: "EncodeButtonFragment_encode_profiles") {
      edges {
        node {
          id
          name
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

  const query = useLazyLoadQuery<EncodeButtonQuery>(QUERY, {})
  const { data } = usePaginationFragment<EncodeButtonQuery, EncodeButtonFragment$key>(FRAGMENT, query)

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

  const onPress = (key: Selection) => {
    commit({
      variables: {
        input: {
          encode_profile_id: key,
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

      <Menu.List placement="bottom right" onAction={(key) => onPress(key)}>
        {data.encode_profiles?.edges?.map((profile) => (
          <Menu.Item key={profile.node.id} className="flex items-center gap-2" id={profile.node.id}>
            {profile.node.name}

            <div className="flex items-end flex-grow justify-end gap-2">
              <Chip color="success" size="sm">
                {profile.node.codec.name.toLocaleLowerCase()}
              </Chip>

              <Chip color="info" size="sm">
                {profile.node.preset.name.toLocaleLowerCase()}
              </Chip>

              {profile.node.tune && (
                <Chip color="warning" size="sm">
                  {profile.node.tune.name.toLocaleLowerCase()}
                </Chip>
              )}
            </div>
          </Menu.Item>
        ))}
      </Menu.List>
    </Menu>
  )
}

export default EncodeButton
