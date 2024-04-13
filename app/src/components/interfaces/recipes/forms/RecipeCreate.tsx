import type { RecipeCreateMutation } from '@/__generated__/RecipeCreateMutation.graphql'
import type { RecipeFormRef, RecipeInput } from '@/components/interfaces/recipes/forms'
import type { SubmitHandler } from 'react-hook-form'

import { Alert } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import React from 'react'
import { useMutation } from 'react-relay'
import { ConnectionHandler, ROOT_ID, graphql } from 'relay-runtime'

import RecipeForm from '@/components/interfaces/recipes/forms/RecipeForm'

const CONNECTION = ConnectionHandler.getConnectionID(ROOT_ID, 'RecipeTableFragment_recipes', [])

const MUTATION = graphql`
  mutation RecipeCreateMutation($connections: [ID!]!, $input: Recipe_createInput!) {
    recipe_create(input: $input) {
      recipe @appendNode(connections: $connections, edgeTypeName: "RecipesEdge") {
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

type RecipeCreateProps = {
  onComplete?: (payload: any) => void
  onLoadingChange?: (isLoading: boolean) => void
}

const RecipeCreate = React.forwardRef<RecipeFormRef, RecipeCreateProps>((props, ref) => {
  const { onComplete, onLoadingChange } = props

  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<RecipeCreateMutation>(MUTATION)

  const onSubmit: SubmitHandler<RecipeInput> = React.useCallback(
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
          if (payload.recipe_create.errors != null) {
            const faults = payload.recipe_create.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (payload.recipe_create.recipe) onComplete?.(payload.recipe_create.recipe)

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

      <RecipeForm ref={ref} onSubmit={onSubmit} />
    </div>
  )
})

RecipeCreate.defaultProps = {
  onComplete: undefined,
  onLoadingChange: undefined,
}

export default RecipeCreate
