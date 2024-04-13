import type { RecipeUpdateMutation } from '@/__generated__/RecipeUpdateMutation.graphql'
import type { RecipeFormRef, RecipeInput } from '@/components/interfaces/recipes/forms'
import type { SubmitHandler } from 'react-hook-form'

import { Alert } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import React from 'react'
import { useMutation } from 'react-relay'
import { graphql } from 'relay-runtime'

import RecipeForm from '@/components/interfaces/recipes/forms/RecipeForm'

const MUTATION = graphql`
  mutation RecipeUpdateMutation($input: Recipe_updateInput!) {
    recipe_update(input: $input) {
      recipe {
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

export type RecipeUpdateInput = RecipeInput & {
  id: string
}

type RecipeUpdateProps = {
  recipe: RecipeUpdateInput
  onComplete?: (payload: any) => void
  onLoadingChange?: (isLoading: boolean) => void
}

const RecipeUpdate = React.forwardRef<RecipeFormRef, RecipeUpdateProps>((props, ref) => {
  const { recipe, onComplete, onLoadingChange } = props

  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<RecipeUpdateMutation>(MUTATION)

  const onSubmit: SubmitHandler<RecipeInput> = React.useCallback(
    (data) => {
      commit({
        variables: {
          input: {
            id: recipe.id,
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
          if (payload.recipe_update.errors != null) {
            const faults = payload.recipe_update.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (payload.recipe_update.recipe) onComplete?.(payload.recipe_update.recipe)

          setErrors([])
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [recipe, commit, onComplete]
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

      <RecipeForm ref={ref} recipe={recipe} onSubmit={onSubmit} />
    </div>
  )
})

RecipeUpdate.defaultProps = {
  onComplete: undefined,
  onLoadingChange: undefined,
}

export default RecipeUpdate
