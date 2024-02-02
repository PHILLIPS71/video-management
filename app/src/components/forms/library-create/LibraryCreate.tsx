'use client'

import type {
  LibraryCreate_LibraryCreateMutation,
  LibraryCreate_LibraryCreateMutation$data,
} from '@/__generated__/LibraryCreate_LibraryCreateMutation.graphql'
import type { SubmitHandler } from 'react-hook-form'

import { Alert, Button, Form, Input, Switch, Typography } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { IconAlertCircleFilled, IconAlertTriangleFilled } from '@tabler/icons-react'
import React from 'react'
import { useForm } from 'react-hook-form'
import { ConnectionHandler, graphql, useMutation } from 'react-relay'
import { ROOT_ID } from 'relay-runtime'
import * as z from 'zod'

type LibraryCreateInput = z.infer<typeof LibraryCreateSchema>

type LibraryCreateProps = {
  onComplete?: (payload: LibraryCreatePayload) => void
}

export type LibraryCreatePayload = NonNullable<LibraryCreate_LibraryCreateMutation$data['library_create']['library']>

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

const SlugTransform = z.string().transform((val) =>
  val
    ?.toLowerCase()
    .replaceAll(' ', '-')
    .replace(/[^a-z0-9-]+/g, '')
)

const LibraryCreateSchema = z.object({
  name: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  slug: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  path: z.string().trim().min(1, { message: 'not enough chars' }),
  is_watched: z.boolean().default(true),
})

const LibraryCreate: React.FC<LibraryCreateProps> = ({ onComplete }) => {
  const form = useForm<LibraryCreateInput>({ resolver: zodResolver(LibraryCreateSchema) })
  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<LibraryCreate_LibraryCreateMutation>(MUTATION)

  const onSubmit: SubmitHandler<LibraryCreateInput> = (data) => {
    const connection = ConnectionHandler.getConnectionID(ROOT_ID, 'SidebarLibrarySegmentFragment_libraries', [])

    commit({
      variables: {
        connections: [connection],
        input: {
          name: data.name,
          slug: SlugTransform.parse(data.slug),
          path: data.path,
          // is_watched: data.is_watched,
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

        if (onComplete && payload.library_create.library) onComplete(payload.library_create.library)
      },
      onError: (error) => {
        setErrors([error.message])
      },
    })
  }

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
          <Form.Group error={!!form.formState.errors.name}>
            <Form.Label htmlFor="name">Name</Form.Label>
            <Input>
              <Input.Control id="name" type="text" {...form.register('name')} />
            </Input>
            <Form.Feedback type="error">{form.formState.errors.name?.message}!!!</Form.Feedback>
          </Form.Group>

          <Form.Group error={!!form.formState.errors.slug}>
            <Form.Label htmlFor="slug">Slug</Form.Label>
            <Input>
              <Input.Control id="slug" type="text" {...form.register('slug')} />
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

        <Form.Group error={!!form.formState.errors.path}>
          <Form.Label htmlFor="folder">Folder</Form.Label>
          <Input>
            <Input.Control id="folder" type="text" {...form.register('path')} />
          </Input>
          <Form.Feedback type="error">{form.formState.errors.path?.message}</Form.Feedback>
        </Form.Group>

        <Form.Group error={!!form.formState.errors.is_watched}>
          <span className="flex gap-2 items-center">
            <Form.Label className="m-0" htmlFor="is-watched">
              <Switch id="is-watched" {...form.register('is_watched')} />
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

        <Button className="self-end" color="brand" disabled={isLoading} size="xs" type="submit">
          Create library
        </Button>
      </div>
    </Form>
  )
}

LibraryCreate.defaultProps = {
  onComplete: undefined,
}

export default LibraryCreate
