import type { EncodeButton_EncodeSubmitMutation } from '@/__generated__/EncodeButton_EncodeSubmitMutation.graphql'

import { Button } from '@giantnodes/react'
import React from 'react'
import { graphql, useMutation } from 'react-relay'

type EncodeButtonProps = {
  paths: Set<string>
}

const MUTATION = graphql`
  mutation EncodeButton_EncodeSubmitMutation($input: File_encode_submitInput!) {
    file_encode_submit(input: $input) {
      encode {
        id
        file {
          ...ExploreTableFileFragment
        }
      }
    }
  }
`

const EncodeButton: React.FC<EncodeButtonProps> = ({ directory_id, keys }) => {
  const [commit, isLoading] = useMutation<EncodeButton_EncodeSubmitMutation>(MUTATION)

  const onClick = () => {
    commit({
      variables: {
        input: {
          entries: Array.from(paths.values()),
        },
      },
    })
  }

  return (
    <Button color="brand" disabled={paths.size === 0 || isLoading} size="xs" onClick={() => onClick()}>
      Encode
    </Button>
  )
}

export default EncodeButton
