import type { EncodeProfileCreateMutation } from '@/__generated__/EncodeProfileCreateMutation.graphql'
import type { EncodeProfileFormRef, EncodeProfileInput } from '@/components/interfaces/profiles/forms'
import type { SubmitHandler } from 'react-hook-form'

import { Alert } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import React from 'react'
import { useMutation } from 'react-relay'
import { ConnectionHandler, ROOT_ID, graphql } from 'relay-runtime'

import EncodeProfileForm from '@/components/interfaces/profiles/forms/EncodeProfileForm'

const CONNECTION = ConnectionHandler.getConnectionID(ROOT_ID, 'EncodeProfileTableFragment_encode_profiles', [])

const MUTATION = graphql`
  mutation EncodeProfileCreateMutation($connections: [ID!]!, $input: Encode_profile_createInput!) {
    encode_profile_create(input: $input) {
      encodeProfile @appendNode(connections: $connections, edgeTypeName: "EncodeProfilesEdge") {
        id
        name
        quality
        use_hardware_acceleration
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

type EncodeProfileCreateProps = {
  onComplete?: (payload: any) => void
  onLoadingChange?: (isLoading: boolean) => void
}

const EncodeProfileCreate = React.forwardRef<EncodeProfileFormRef, EncodeProfileCreateProps>((props, ref) => {
  const { onComplete, onLoadingChange } = props

  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<EncodeProfileCreateMutation>(MUTATION)

  const onSubmit: SubmitHandler<EncodeProfileInput> = React.useCallback(
    (data) => {
      commit({
        variables: {
          connections: [CONNECTION],
          input: {
            name: data.name,
            container: data.container,
            codec: data.codec,
            preset: data.preset,
            tune: data.tune,
            quality: data.quality,
            use_hardware_acceleration: data.use_hardware_acceleration,
          },
        },
        onCompleted: (payload) => {
          if (payload.encode_profile_create.errors != null) {
            const faults = payload.encode_profile_create.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (payload.encode_profile_create.encodeProfile) onComplete?.(payload.encode_profile_create.encodeProfile)

          setErrors([])
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [commit, onComplete]
  )

  React.useEffect(() => {
    onLoadingChange?.(isLoading)
  }, [isLoading, onLoadingChange])

  return (
    <div>
      {errors.length > 0 && (
        <Alert color="danger">
          <IconAlertCircleFilled size={16} />
          <Alert.Body>
            <Alert.Heading>There were {errors.length} error with your submission</Alert.Heading>
            <Alert.List>
              {errors.map((error) => (
                <Alert.Item key={error}>{error}</Alert.Item>
              ))}
            </Alert.List>
          </Alert.Body>
        </Alert>
      )}

      <EncodeProfileForm ref={ref} onSubmit={onSubmit} />
    </div>
  )
})

EncodeProfileCreate.defaultProps = {
  onComplete: undefined,
  onLoadingChange: undefined,
}

export default EncodeProfileCreate
