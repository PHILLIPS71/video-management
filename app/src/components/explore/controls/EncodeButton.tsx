import type { EncodeButton_EncodeSubmitMutation } from '@/__generated__/EncodeButton_EncodeSubmitMutation.graphql'

import { Button } from '@giantnodes/react'
import React from 'react'
import { graphql, useMutation } from 'react-relay'

import { useExploreContext } from '@/components/explore/use-explore.hook'

const MUTATION = graphql`
  mutation EncodeButton_EncodeSubmitMutation($input: File_encode_submitInput!) {
    file_encode_submit(input: $input) {
      encode {
        id
      }
    }
  }
`

const EncodeButton: React.FC = () => {
  const { directory, keys } = useExploreContext()

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

  const onClick = () => {
    commit({
      variables: {
        input: {
          entries,
        },
      },
    })
  }

  return (
    <Button color="brand" isDisabled={isDisabled || isLoading} size="xs" onPress={() => onClick()}>
      Encode
    </Button>
  )
}

export default EncodeButton
