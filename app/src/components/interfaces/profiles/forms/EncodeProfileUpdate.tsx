import type { EncodeProfileUpdateMutation } from '@/__generated__/EncodeProfileUpdateMutation.graphql'
import type { EncodeProfileFormRef, EncodeProfileInput } from '@/components/interfaces/profiles/forms'
import type { SubmitHandler } from 'react-hook-form'

import { Alert } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import React from 'react'
import { useMutation } from 'react-relay'
import { graphql } from 'relay-runtime'

import EncodeProfileForm from '@/components/interfaces/profiles/forms/EncodeProfileForm'

const MUTATION = graphql`
  mutation EncodeProfileUpdateMutation($input: Encode_profile_updateInput!) {
    encode_profile_update(input: $input) {
      encodeProfile {
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

export type EncodeProfileUpdateInput = EncodeProfileInput & {
  id: string
}

type EncodeProfileUpdateProps = {
  profile: EncodeProfileUpdateInput
  onComplete?: (payload: any) => void
  onLoadingChange?: (isLoading: boolean) => void
}

const EncodeProfileUpdate = React.forwardRef<EncodeProfileFormRef, EncodeProfileUpdateProps>((props, ref) => {
  const { profile, onComplete, onLoadingChange } = props

  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<EncodeProfileUpdateMutation>(MUTATION)

  const onSubmit: SubmitHandler<EncodeProfileInput> = React.useCallback(
    (data) => {
      commit({
        variables: {
          input: {
            id: profile.id,
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
          if (payload.encode_profile_update.errors != null) {
            const faults = payload.encode_profile_update.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (payload.encode_profile_update.encodeProfile) onComplete?.(payload.encode_profile_update.encodeProfile)

          setErrors([])
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [profile, commit, onComplete]
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

      <EncodeProfileForm ref={ref} profile={profile} onSubmit={onSubmit} />
    </div>
  )
})

EncodeProfileUpdate.defaultProps = {
  onComplete: undefined,
  onLoadingChange: undefined,
}

export default EncodeProfileUpdate
