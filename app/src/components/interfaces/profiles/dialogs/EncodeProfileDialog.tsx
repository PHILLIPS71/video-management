import type { EncodeProfileDialog_DeleteEncodeProfileMutation } from '@/__generated__/EncodeProfileDialog_DeleteEncodeProfileMutation.graphql'
import type { EncodeProfileFormRef, EncodeProfileUpdateInput } from '@/components/interfaces/profiles'

import { Alert, Button, Card, Dialog, Divider, Typography } from '@giantnodes/react'
import { IconAlertCircleFilled, IconTrash, IconX } from '@tabler/icons-react'
import React, { Suspense } from 'react'
import { graphql, useMutation } from 'react-relay'

import { EncodeProfileCreate, EncodeProfileUpdate } from '@/components/interfaces/profiles/forms'

const MUTATION = graphql`
  mutation EncodeProfileDialog_DeleteEncodeProfileMutation($input: Encode_profile_deleteInput!) {
    encode_profile_delete(input: $input) {
      encodeProfile {
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

type EncodeProfileDialogProps = React.PropsWithChildren & {
  profile?: EncodeProfileUpdateInput
}

const EncodeProfileDialog: React.FC<EncodeProfileDialogProps> = ({ children, profile }) => {
  const ref = React.useRef<EncodeProfileFormRef>(null)

  const [errors, setErrors] = React.useState<string[]>([])
  const [isSaveLoading, setSaveLoading] = React.useState<boolean>(false)

  const [commit, isDeleteLoading] = useMutation<EncodeProfileDialog_DeleteEncodeProfileMutation>(MUTATION)

  const remove = React.useCallback(
    (entry: EncodeProfileUpdateInput, onComplete: () => void) => {
      commit({
        variables: {
          input: {
            id: entry.id,
          },
        },
        onCompleted: (payload) => {
          if (payload.encode_profile_delete.errors != null) {
            const faults = payload.encode_profile_delete.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          setErrors([])
          onComplete()
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [commit]
  )

  return (
    <Dialog placement="right">
      {children}

      <Dialog.Content>
        {({ close }) => (
          <Card>
            <Card.Header>
              <div className="flex flex-col gap-3">
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

                <div className="flex items-center justify-between">
                  <Typography.HeadingLevel>
                    <Typography.Heading as={6}>{profile?.name ?? 'Create new profile'}</Typography.Heading>
                  </Typography.HeadingLevel>

                  <div className="flex items-center gap-3">
                    {profile && (
                      <Button
                        color="danger"
                        isDisabled={isDeleteLoading}
                        size="xs"
                        onPress={() => remove(profile, close)}
                      >
                        <IconTrash size={16} />
                      </Button>
                    )}

                    <Divider orientation="horizontal" />

                    <Button color="transparent" size="none" onPress={close}>
                      <IconX size={22} strokeWidth={1} />
                    </Button>
                  </div>
                </div>
              </div>
            </Card.Header>

            <Card.Body>
              <Suspense fallback="Loading...">
                {profile ? (
                  <EncodeProfileUpdate
                    ref={ref}
                    profile={profile}
                    onComplete={close}
                    onLoadingChange={setSaveLoading}
                  />
                ) : (
                  <EncodeProfileCreate ref={ref} onComplete={close} onLoadingChange={setSaveLoading} />
                )}
              </Suspense>
            </Card.Body>

            <Card.Footer className="flex items-center justify-end gap-3">
              <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
                Reset
              </Button>
              <Button isDisabled={isSaveLoading} size="xs" onPress={() => ref.current?.submit()}>
                Save
              </Button>
            </Card.Footer>
          </Card>
        )}
      </Dialog.Content>
    </Dialog>
  )
}

EncodeProfileDialog.defaultProps = {
  profile: undefined,
}

export default EncodeProfileDialog
