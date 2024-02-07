'use client'

import type {
  LibraryCreate_LibraryCreateMutation,
  LibraryCreate_LibraryCreateMutation$data,
} from '@/__generated__/LibraryCreate_LibraryCreateMutation.graphql'
import type { SubmitHandler } from 'react-hook-form'

import { Alert, Form, Input, Switch, Typography } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { IconAlertCircleFilled, IconAlertTriangleFilled } from '@tabler/icons-react'
import React from 'react'
import { useForm } from 'react-hook-form'
import { ConnectionHandler, graphql, useMutation } from 'react-relay'
import { ROOT_ID } from 'relay-runtime'
import * as z from 'zod'

import { SlugTransform } from '@/components/forms/library/LibraryForms.constants'

export type LibraryCreateRef = {
  submit: () => void
  reset: () => void
}

export type LibraryCreatePayload = NonNullable<LibraryCreate_LibraryCreateMutation$data['library_create']['library']>

type LibraryCreateInput = z.infer<typeof LibraryCreateSchema>

type LibraryCreateProps = {
  onComplete?: (payload: LibraryCreatePayload) => void
  onLoadingChange?: (isLoading: boolean) => void
}

const MUTATION = graphql`
  mutation LibraryCreate_LibraryCreateMutation($connections: [ID!]!, $input: Library_createInput!) {
    library_create(input: $input) {
      library @appendNode(connections: $connections, edgeTypeName: "LibrariesEdge") {
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

const LibraryCreateSchema = z.object({
  name: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  slug: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  path: z.string().trim().min(1, { message: 'not enough chars' }),
  is_watched: z.boolean().default(true),
})

const LibraryCreate = React.forwardRef<LibraryCreateRef, LibraryCreateProps>((props, ref) => {
  const { onComplete, onLoadingChange } = props

  const [errors, setErrors] = React.useState<string[]>([])
  const [commit, isLoading] = useMutation<LibraryCreate_LibraryCreateMutation>(MUTATION)

  const form = useForm<LibraryCreateInput>({ resolver: zodResolver(LibraryCreateSchema) })

  const onSubmit: SubmitHandler<LibraryCreateInput> = React.useCallback(
    (data) => {
      const connection = ConnectionHandler.getConnectionID(ROOT_ID, 'SidebarLibrarySegmentFragment_libraries', [])

      commit({
        variables: {
          connections: [connection],
          input: {
            name: data.name,
            slug: SlugTransform.parse(data.slug),
            path: data.path,
            is_watched: data.is_watched,
          },
        },
        onCompleted: (payload) => {
          if (payload.library_create.errors != null) {
            const faults = payload.library_create.errors
              .filter((error) => error.message !== undefined)
              .map((error) => error.message!)

            setErrors(faults)

            return
          }

          if (payload.library_create.library) onComplete?.(payload.library_create.library)

          setErrors([])
        },
        onError: (error) => {
          setErrors([error.message])
        },
      })
    },
    [commit, onComplete]
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

  React.useEffect(() => {
    onLoadingChange?.(isLoading)
  }, [isLoading, onLoadingChange])

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

        <Form.Group {...form.register('path')} error={!!form.formState.errors.path}>
          <Form.Label>Folder</Form.Label>
          <Input>
            <Input.Control type="text" />
          </Input>
          <Form.Feedback type="error">{form.formState.errors.path?.message}</Form.Feedback>
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

LibraryCreate.defaultProps = {
  onComplete: undefined,
  onLoadingChange: undefined,
}

export default LibraryCreate
