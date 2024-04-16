import type { RecipeFormQuery } from '@/__generated__/RecipeFormQuery.graphql'
import type { SubmitHandler } from 'react-hook-form'

import { Form, Input, Select, Switch, Typography } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import React from 'react'
import { useForm, useWatch } from 'react-hook-form'
import { useLazyLoadQuery } from 'react-relay'
import { graphql } from 'relay-runtime'
import * as z from 'zod'

const QUERY = graphql`
  query RecipeFormQuery {
    encode_codecs {
      nodes {
        id
        name
        value
        description
        quality {
          min
          max
        }
        tunes {
          id
          name
          value
          description
        }
      }
    }
    encode_presets {
      nodes {
        id
        name
        description
      }
    }
    video_file_containers {
      nodes {
        id
        extension
      }
    }
  }
`

const RecipeSchema = z.object({
  name: z.string().trim().min(1).max(128),
  container: z.string().nullish(),
  codec: z.string(),
  preset: z.string(),
  tune: z.string().nullish(),
  quality: z.preprocess((x) => x || null, z.coerce.number().int().nullish()),
  use_hardware_acceleration: z.boolean().default(false),
})

export type RecipeFormRef = {
  submit: () => void
  reset: () => void
}

export type RecipeInput = z.infer<typeof RecipeSchema>

type RecipeFormProps = {
  recipe?: RecipeInput
  onSubmit: SubmitHandler<RecipeInput>
}

const RecipeForm = React.forwardRef<RecipeFormRef, RecipeFormProps>((props, ref) => {
  const { recipe, onSubmit } = props
  const { encode_codecs, encode_presets, video_file_containers } = useLazyLoadQuery<RecipeFormQuery>(QUERY, {})

  const form = useForm<RecipeInput>({
    resolver: zodResolver(RecipeSchema),
    defaultValues: {
      name: recipe?.name,
      container: recipe?.container,
      codec: recipe?.codec,
      preset: recipe?.preset,
      tune: recipe?.tune,
      quality: recipe?.quality,
      use_hardware_acceleration: recipe?.use_hardware_acceleration,
    },
  })

  const codecId = useWatch({ control: form.control, name: 'codec' })

  const codec = React.useMemo(
    () => encode_codecs?.nodes?.find((x) => x.id === codecId),
    [codecId, encode_codecs?.nodes]
  )

  React.useEffect(() => {
    const subscription = form.watch((_, { name }) => {
      if (name !== 'codec') return

      form.setValue('tune', null)
    })

    return () => subscription.unsubscribe()
  }, [form, form.watch])

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
    <Form>
      <div className="flex flex-col gap-y-3">
        <Form.Group {...form.register('name')} error={!!form.formState.errors.name}>
          <Form.Label>Name</Form.Label>
          <Input>
            <Input.Control type="text" />
          </Input>
          <Form.Feedback type="error">{form.formState.errors.name?.message}</Form.Feedback>
        </Form.Group>

        <div className="flex flex-row items-center gap-3 flex-wrap sm:flex-nowrap">
          <Form.Group {...form.register('codec')} error={!!form.formState.errors.codec}>
            <Form.Label>Codec</Form.Label>
            <Select items={encode_codecs?.nodes} selectedKey={form.watch('codec')}>
              {(item) => (
                <Select.Option key={item.id} id={item.id} textValue={item.name}>
                  <div className="flex flex-col">
                    <Typography.Text>{item.name}</Typography.Text>
                    <Typography.Text variant="subtitle">{item.description}</Typography.Text>
                  </div>
                </Select.Option>
              )}
            </Select>
            <Form.Feedback type="error">{form.formState.errors.codec?.message}</Form.Feedback>
          </Form.Group>

          <Form.Group {...form.register('container')} error={!!form.formState.errors.container}>
            <Form.Label>Container</Form.Label>
            <Select items={video_file_containers?.nodes} selectedKey={form.watch('container')}>
              {(item) => (
                <Select.Option key={item.id} id={item.id}>
                  {item.extension}
                </Select.Option>
              )}
            </Select>
            <Form.Feedback type="error">{form.formState.errors.container?.message}</Form.Feedback>
          </Form.Group>
        </div>

        <div className="flex flex-row items-center gap-3 flex-wrap sm:flex-nowrap">
          <Form.Group {...form.register('preset')} error={!!form.formState.errors.preset}>
            <Form.Label>Preset</Form.Label>
            <Select items={encode_presets?.nodes} selectedKey={form.watch('preset')}>
              {(item) => (
                <Select.Option key={item.id} id={item.id} textValue={item.name}>
                  <div className="flex flex-col">
                    <Typography.Text>{item.name}</Typography.Text>
                    <Typography.Text variant="subtitle">{item.description}</Typography.Text>
                  </div>
                </Select.Option>
              )}
            </Select>
            <Form.Feedback type="error">{form.formState.errors.preset?.message}</Form.Feedback>
          </Form.Group>

          <Form.Group {...form.register('quality')} error={!!form.formState.errors.quality}>
            <Form.Label>Quality</Form.Label>
            <Input>
              <Input.Control max={codec?.quality.max} min={codec?.quality.min ?? 1} type="number" />
            </Input>
            <Form.Feedback type="error">{form.formState.errors.quality?.message}</Form.Feedback>
          </Form.Group>
        </div>

        <div className="flex flex-row items-center gap-3 flex-wrap sm:flex-nowrap">
          <Form.Group {...form.register('tune')} error={!!form.formState.errors.tune}>
            <Form.Label>Tune</Form.Label>
            <Select isDisabled={codec?.tunes.length === 0} items={codec?.tunes} selectedKey={form.watch('tune')}>
              {(item) => (
                <Select.Option key={item.id} id={item.id} textValue={item.name}>
                  <div className="flex flex-col">
                    <Typography.Text>{item.name}</Typography.Text>
                    <Typography.Text variant="subtitle">{item.description}</Typography.Text>
                  </div>
                </Select.Option>
              )}
            </Select>
            <Form.Feedback type="error">{form.formState.errors.tune?.message}</Form.Feedback>
          </Form.Group>

          <Form.Group
            {...form.register('use_hardware_acceleration')}
            error={!!form.formState.errors.use_hardware_acceleration}
          >
            <Form.Label>Use Hardware Acceleration</Form.Label>
            <Switch />
            <Form.Feedback type="error">{form.formState.errors.use_hardware_acceleration?.message}</Form.Feedback>
          </Form.Group>
        </div>
      </div>
    </Form>
  )
})

RecipeForm.defaultProps = {
  recipe: undefined,
}

export default RecipeForm
