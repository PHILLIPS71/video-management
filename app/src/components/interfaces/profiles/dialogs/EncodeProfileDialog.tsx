import type { EncodeProfileFormRef, EncodeProfileInput } from '@/components/interfaces/profiles'

import { Button, Card, Dialog, Divider, Typography } from '@giantnodes/react'
import { IconTrash, IconX } from '@tabler/icons-react'
import React, { Suspense } from 'react'

import { EncodeProfileCreate, EncodeProfileUpdate } from '@/components/interfaces/profiles/forms'

type EncodeProfileDialogProps = React.PropsWithChildren & {
  profile?: EncodeProfileInput
}

const EncodeProfileDialog: React.FC<EncodeProfileDialogProps> = ({ children, profile }) => {
  const ref = React.useRef<EncodeProfileFormRef>(null)

  const [isLoading, setLoading] = React.useState<boolean>(false)

  return (
    <Dialog placement="right">
      {children}

      <Dialog.Content>
        {({ close }) => (
          <Card>
            <Card.Header>
              <div className="flex items-center justify-between">
                <Typography.HeadingLevel>
                  <Typography.Heading as={6}>{profile?.name ?? 'Create new profile'}</Typography.Heading>
                </Typography.HeadingLevel>

                <div className="flex items-center gap-3">
                  {profile && (
                    <Button color="danger" size="xs">
                      <IconTrash size={16} />
                    </Button>
                  )}

                  <Divider orientation="horizontal" />

                  <Button color="transparent" size="none" onPress={close}>
                    <IconX size={22} strokeWidth={1} />
                  </Button>
                </div>
              </div>
            </Card.Header>

            <Card.Body>
              <Suspense fallback="Loading...">
                {profile ? (
                  <EncodeProfileUpdate ref={ref} profile={profile} onLoadingChange={setLoading} />
                ) : (
                  <EncodeProfileCreate ref={ref} onLoadingChange={setLoading} />
                )}
              </Suspense>
            </Card.Body>

            <Card.Footer className="flex items-center justify-end gap-3">
              <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
                Reset
              </Button>
              <Button isDisabled={isLoading} size="xs" onPress={() => ref.current?.submit()}>
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
