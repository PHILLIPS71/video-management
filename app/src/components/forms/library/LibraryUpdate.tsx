'use client'

import type {
  LibraryUpdate_LibraryUpdateMutation,
  LibraryUpdate_LibraryUpdateMutation$data,
} from '@/__generated__/LibraryUpdate_LibraryUpdateMutation.graphql'
import type { SubmitHandler } from 'react-hook-form'

import { Alert, Form, Input, Switch, Typography } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { IconAlertCircleFilled, IconAlertTriangleFilled } from '@tabler/icons-react'
import React from 'react'
import { useForm } from 'react-hook-form'
import { graphql, useMutation } from 'react-relay'
import * as z from 'zod'

export type LibraryUpdateRef = {
  submit: () => void
  reset: () => void
}

export type LibraryUpdatePayload = NonNullable<LibraryUpdate_LibraryUpdateMutation$data['library_update']['library']>

type LibraryUpdateInput = z.infer<typeof LibraryUpdateSchema>

type LibraryUpdateProps = {
  library: {
    id: string
    name: string
    slug: string
    is_watched: boolean
    path_info: {
      full_name: string
    }
  }
  onComplete?: (payload: LibraryUpdatePayload) => void
}

const MUTATION = graphql`
  mutation LibraryUpdate_LibraryUpdateMutation($input: Library_updateInput!) {
    library_update(input: $input) {
      library {
        slug
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

const SlugTransform = z.string().transform((val) =>
  val
    ?.toLowerCase()
    .replaceAll(' ', '-')
    .replace(/[^a-z0-9-]+/g, '')
)

const LibraryUpdateSchema = z.object({
  name: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  slug: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  is_watched: z.boolean().default(true),
})

const LibraryUpdate = React.forwardRef<LibraryUpdateRef, LibraryUpdateProps>((props, ref) => {
  const { library, onComplete } = props

  const [errors, setErrors] = React.useState<string[]>([])
  const [commit] = useMutation<LibraryUpdate_LibraryUpdateMutation>(MUTATION)

  const form = useForm<LibraryUpdateInput>({
    resolver: zodResolver(LibraryUpdateSchema),
    defaultValues: {
      name: library.name,
      slug: library.slug,
      is_watched: library.is_watched,
    },
  })

  const onSubmit: SubmitHandler<LibraryUpdateInput> = React.useCallback(
    (data) => {
      commit({
        variables: {
          input: {
            id: library.id,
            name: data.name,
            slug: SlugTransform.parse(data.slug),
            is_watched: data.is_watched,
          },
        },
        onCompleted: (payload) => {
          if (payload.library_update.errors != null) {
            const faults = payload.library_update.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (onComplete && payload.library_update.library) onComplete(payload.library_update.library)
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [library.id, commit, onComplete]
  )

  React.useImperativeHandle(
    ref,
    () => ({
      submit: () => {
        form.handleSubmit(onSubmit)()
      },
      reset: () => {
        form.reset()
      },
    }),
    [form, onSubmit]
  )

  return (
    <Form onSubmit={form.handleSubmit(onSubmit)}>
      <div className="flex flex-col gap-y-4">
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

        <div className="flex flex-row gap-4 flex-wrap md:flex-nowrap">
          <Form.Group {...form.register('name')} error={!!form.formState.errors.name}>
            <Form.Label>Name</Form.Label>
            <Input>
              <Input.Control type="text" />
            </Input>
            <Form.Feedback type="error">{form.formState.errors.name?.message}!!!</Form.Feedback>
          </Form.Group>

          <Form.Group {...form.register('slug')} error={!!form.formState.errors.slug}>
            <Form.Label>Slug</Form.Label>
            <Input>
              <Input.Control type="text" />
            </Input>
            <Form.Feedback type="error">{form.formState.errors.slug?.message}</Form.Feedback>
            {form.watch('slug') !== undefined && SlugTransform.parse(form.watch('slug')) !== form.watch('slug') && (
              <Form.Caption className="text-yellow-600">
                <IconAlertTriangleFilled className="inline-block" size={12} /> The library slug can only contain ASCII
                letters and digits so it will be created as <strong>{SlugTransform.parse(form.watch('slug'))}</strong>.
              </Form.Caption>
            )}
          </Form.Group>
        </div>

        <Form.Group>
          <Form.Label>Folder</Form.Label>
          <Input>
            <Input.Control disabled type="text" value={library.path_info.full_name} />
          </Input>
        </Form.Group>

        <Form.Group error={!!form.formState.errors.is_watched}>
          <span className="flex gap-2 items-center">
            <Form.Label className="m-0">
              <Switch {...form.register('is_watched')} />
            </Form.Label>

            <div className="flex flex-col">
              <Typography.Paragraph className="font-semibold">
                Monitor library folder
                <Typography.Text className="pl-1" variant="subtitle">
                  (recommended)
                </Typography.Text>
              </Typography.Paragraph>

              <Typography.Paragraph variant="subtitle">
                Watches the library folder and sub-directories for file system changes and automatically updates the
                library.
              </Typography.Paragraph>
            </div>
          </span>

          <Form.Feedback type="error">{form.formState.errors.is_watched?.message}</Form.Feedback>
        </Form.Group>
      </div>
    </Form>
  )
})

LibraryUpdate.defaultProps = {
  onComplete: undefined,
}

export default LibraryUpdate
