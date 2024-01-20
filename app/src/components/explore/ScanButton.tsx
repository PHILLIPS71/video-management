import type { ScanButton_DirectoryProbeMutation } from '@/__generated__/ScanButton_DirectoryProbeMutation.graphql'

import { Button } from '@giantnodes/react'
import { IconFolderSearch } from '@tabler/icons-react'
import { graphql, useMutation } from 'react-relay'

type ScanButtonProps = {
  directory_id: string
}

const MUTATION = graphql`
  mutation ScanButton_DirectoryProbeMutation($input: Directory_probeInput!) {
    directory_probe(input: $input) {
      string
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

const ScanButton: React.FC<ScanButtonProps> = ({ directory_id }) => {
  const [commit, isLoading] = useMutation<ScanButton_DirectoryProbeMutation>(MUTATION)

  const onClick = () => {
    commit({
      variables: {
        input: {
          directory_id,
        },
      },
    })
  }

  return (
    <Button color="brand" disabled={isLoading} size="xs" onClick={() => onClick()}>
      <IconFolderSearch size={16} /> Scan
    </Button>
  )
}

export default ScanButton
