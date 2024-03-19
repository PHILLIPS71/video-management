import type { ScanButton_DirectoryProbeMutation } from '@/__generated__/ScanButton_DirectoryProbeMutation.graphql'

import { Button } from '@giantnodes/react'
import { IconFolderSearch } from '@tabler/icons-react'
import { graphql, useMutation } from 'react-relay'

import { useExploreContext } from '@/components/interfaces/explore/use-explore.hook'

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

const ScanButton: React.FC = () => {
  const { directory, setErrors } = useExploreContext()

  const [commit, isLoading] = useMutation<ScanButton_DirectoryProbeMutation>(MUTATION)

  const onPress = () => {
    commit({
      variables: {
        input: {
          directory_id: directory,
        },
      },
      onCompleted: (payload) => {
        if (payload.directory_probe.errors != null) {
          const faults = payload.directory_probe.errors
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
    <Button color="brand" disabled={isLoading} size="xs" onPress={() => onPress()}>
      <IconFolderSearch size={16} /> Scan
    </Button>
  )
}

export default ScanButton
