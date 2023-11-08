'use client'

import type { page_LibraryCreate_LibraryCreateMutation } from '@/__generated__/page_LibraryCreate_LibraryCreateMutation.graphql'
import type { SubmitHandler } from 'react-hook-form'

import { Alert, Button, Card, Form, Input, Typography } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { IconAlertCircleFilled, IconAlertTriangleFilled } from '@tabler/icons-react'
import { useRouter } from 'next/navigation'
import React from 'react'
import { useForm } from 'react-hook-form'
import { ConnectionHandler, graphql, useMutation } from 'react-relay'
import { ROOT_ID } from 'relay-runtime'
import * as z from 'zod'

const SlugTransform = z.string().transform(
  (val) =>
    val
      ?.toLowerCase()
      .replaceAll(' ', '-')
      .replace(/[^a-z0-9-]+/g, '')
)

const LibraryCreateSchema = z.object({
  name: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  slug: z.string().trim().min(1, { message: 'not enough chars' }).max(128, { message: 'too many chars' }),
  path: z.string().trim().min(1, { message: 'not enough chars' }),
  is_watched: z.boolean().default(false),
})

type LibraryCreateInput = z.infer<typeof LibraryCreateSchema>

const LibraryCreatePage = () => {
  const router = useRouter()
  const form = useForm<LibraryCreateInput>({ resolver: zodResolver(LibraryCreateSchema) })
  const [errors, setErrors] = React.useState<string[]>([])

  const [commit, isLoading] = useMutation<page_LibraryCreate_LibraryCreateMutation>(graphql`
    mutation page_LibraryCreate_LibraryCreateMutation($connections: [ID!]!, $input: Library_createInput!) {
      library_create(input: $input) {
        library @appendNode(connections: $connections, edgeTypeName: "LibrariesEdge") {
          slug
          ...SidebarLibrarySegmentItemFragment
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
  `)

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

        if (payload.library_create.library) {
          router.push(`/library/${payload.library_create.library.slug}`)
        }
      },
      onError: (error) => {
        setErrors([error.message])
      },
    })
  }

  return (
    <section className="mx-auto max-w-2xl">
      <Card>
        <Card.Header>
          <Typography.HeadingLevel>
            <Typography.Heading>Create a new library</Typography.Heading>
            <Typography.Text>
              Your library will have its own dedicated metrics and control panel. A dashboard will be set up so you can
              easily interact with your new library.
            </Typography.Text>
          </Typography.HeadingLevel>
        </Card.Header>
        <Card.Body>
          <Form onSubmit={form.handleSubmit(onSubmit)}>
            <div className="flex flex-col gap-y-4">
              {errors.length > 0 && (
                <Alert color="danger">
                  <IconAlertCircleFilled size={16} />
                  <Alert.Body>
                    <Alert.Heading>There were {errors.length} error with your submission</Alert.Heading>
                    <Alert.List>
                      {errors.map((error) => (
                        <Alert.Item>{error}</Alert.Item>
                      ))}
                    </Alert.List>
                  </Alert.Body>
                </Alert>
              )}

              <div className="flex flex-row gap-4 flex-wrap md:flex-nowrap">
                <Form.Group error={form.formState.errors.name}>
                  <Form.Label htmlFor="name">Name</Form.Label>
                  <Input>
                    <Input.Control id="name" type="text" {...form.register('name')} />
                  </Input>
                  <Form.Feedback type="invalid">{form.formState.errors.name?.message}</Form.Feedback>
                </Form.Group>

                <Form.Group error={form.formState.errors.slug}>
                  <Form.Label htmlFor="slug">Slug</Form.Label>
                  <Input>
                    <Input.Control id="slug" type="text" {...form.register('slug')} />
                  </Input>
                  <Form.Feedback type="invalid">{form.formState.errors.slug?.message}</Form.Feedback>
                  {form.watch('slug') !== undefined &&
                    SlugTransform.parse(form.watch('slug')) !== form.watch('slug') && (
                      <Form.Caption className="text-yellow-600">
                        <IconAlertTriangleFilled className="inline-block" size={12} /> The library slug can only contain
                        ASCII letters and digits so it will be created as{' '}
                        <strong>{SlugTransform.parse(form.watch('slug'))}</strong>.
                      </Form.Caption>
                    )}
                </Form.Group>
              </div>

              <Form.Group error={form.formState.errors.path}>
                <Form.Label htmlFor="folder">Folder</Form.Label>
                <Input>
                  <Input.Control id="folder" type="text" {...form.register('path')} />
                </Input>
                <Form.Feedback type="invalid">{form.formState.errors.path?.message}</Form.Feedback>
              </Form.Group>

              <Button className="self-end" color="primary" disabled={isLoading} size="xs" type="submit">
                Create library
              </Button>
            </div>
          </Form>
        </Card.Body>
      </Card>
    </section>
  )
}

export default LibraryCreatePage
