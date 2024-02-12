'use client'

import type { SubmitHandler } from 'react-hook-form'

import { Form } from '@giantnodes/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useTheme } from 'next-themes'
import React from 'react'
import { useForm } from 'react-hook-form'
import * as z from 'zod'

export type PreferenceSettingsRef = {
  submit: () => void
  reset: () => void
}

export type PreferenceSettingsPayload = z.infer<typeof PreferenceSettingsSchema>

type PreferenceSettingsInput = z.infer<typeof PreferenceSettingsSchema>

type PreferenceSettingsProps = {
  onComplete?: (payload: PreferenceSettingsPayload) => void
}

const PreferenceSettingsSchema = z.object({
  theme: z.string(),
})

const PreferenceSettings = React.forwardRef<PreferenceSettingsRef, PreferenceSettingsProps>((props, ref) => {
  const { onComplete } = props
  const { theme, setTheme } = useTheme()

  const form = useForm<PreferenceSettingsInput>({
    resolver: zodResolver(PreferenceSettingsSchema),
    defaultValues: {
      theme,
    },
  })

  const onSubmit: SubmitHandler<PreferenceSettingsInput> = React.useCallback(
    (data) => {
      setTheme(data.theme)

      onComplete?.({ theme: data.theme })
    },
    [setTheme, onComplete]
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
      <Form.Group error={!!form.formState.errors.theme}>
        <Form.Label>Interface Theme</Form.Label>

        <select {...form.register('theme')}>
          <option value="dark">dark</option>
          <option value="light">light</option>
          <option value="system">system</option>
        </select>

        <Form.Feedback type="error">{form.formState.errors.theme?.message}</Form.Feedback>
      </Form.Group>
    </Form>
  )
})

PreferenceSettings.defaultProps = {
  onComplete: undefined,
}

export default PreferenceSettings
