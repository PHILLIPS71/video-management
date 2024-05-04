import type { RecipeDialog_DeleteRecipeMutation } from '@/__generated__/RecipeDialog_DeleteRecipeMutation.graphql'
import type { RecipeFormRef, RecipeUpdateInput } from '@/components/interfaces/recipes'

import { Alert, Button, Card, Dialog, Divider, Typography } from '@giantnodes/react'
import { IconAlertCircleFilled, IconTrash, IconX } from '@tabler/icons-react'
import React, { Suspense } from 'react'
import { graphql, useMutation } from 'react-relay'

import { RecipeCreate, RecipeUpdate } from '@/components/interfaces/recipes/forms'

const MUTATION = graphql`
  mutation RecipeDialog_DeleteRecipeMutation($input: Recipe_deleteInput!) {
    recipe_delete(input: $input) {
      recipe {
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

type RecipeDialogProps = React.PropsWithChildren & {
  recipe?: RecipeUpdateInput
}

const RecipeDialog: React.FC<RecipeDialogProps> = ({ children, recipe }) => {
  const ref = React.useRef<RecipeFormRef>(null)

  const [errors, setErrors] = React.useState<string[]>([])
  const [isSaveLoading, setSaveLoading] = React.useState<boolean>(false)

  const [commit, isDeleteLoading] = useMutation<RecipeDialog_DeleteRecipeMutation>(MUTATION)

  const remove = React.useCallback(
    (entry: RecipeUpdateInput, onComplete: () => void) => {
      commit({
        variables: {
          input: {
            id: entry.id,
          },
        },
        onCompleted: (payload) => {
          if (payload.recipe_delete.errors != null) {
            const faults = payload.recipe_delete.errors
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
                    <Typography.Heading level={6}>{recipe?.name ?? 'Create new recipe'}</Typography.Heading>
                  </Typography.HeadingLevel>

                  <div className="flex items-center gap-3">
                    {recipe && (
                      <Button
                        color="danger"
                        isLoading={isDeleteLoading}
                        size="xs"
                        onPress={() => remove(recipe, close)}
                      >
                        <IconTrash size={16} />
                      </Button>
                    )}

                    <Divider orientation="horizontal" />

                    <Button color="transparent" size="xs" onPress={close}>
                      <IconX size={16} strokeWidth={1} />
                    </Button>
                  </div>
                </div>
              </div>
            </Card.Header>

            <Card.Body>
              <Suspense fallback="Loading...">
                {recipe ? (
                  <RecipeUpdate ref={ref} recipe={recipe} onComplete={close} onLoadingChange={setSaveLoading} />
                ) : (
                  <RecipeCreate ref={ref} onComplete={close} onLoadingChange={setSaveLoading} />
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

RecipeDialog.defaultProps = {
  recipe: undefined,
}

export default RecipeDialog
