import type { EncodeButton_EncodeSubmitMutation } from '@/__generated__/EncodeButton_EncodeSubmitMutation.graphql'

import { Button } from '@giantnodes/react'
import React from 'react'
import { graphql, useMutation } from 'react-relay'

import { useExploreContext } from '@/components/interfaces/explore/use-explore.hook'

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

  const onPress = () => {
    commit({
      variables: {
        input: {
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
    <Button color="brand" isDisabled={isDisabled || isLoading} size="xs" onPress={() => onPress()}>
      Encode
    </Button>
  )
}

export default EncodeButton
