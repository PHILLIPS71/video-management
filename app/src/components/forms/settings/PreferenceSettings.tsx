'use client'

import type { SubmitHandler } from 'react-hook-form'

import { Form, Select } from '@giantnodes/react'
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
      <Form.Group {...form.register('theme')} error={!!form.formState.errors.theme}>
        <Form.Label>Interface Theme</Form.Label>

        <Select>
          <Select.Option id="dark">Dark</Select.Option>
          <Select.Option id="light">Light</Select.Option>
          <Select.Option id="system">System</Select.Option>
        </Select>

        <Form.Feedback type="error">{form.formState.errors.theme?.message}</Form.Feedback>
      </Form.Group>
    </Form>
  )
})

PreferenceSettings.defaultProps = {
  onComplete: undefined,
}

export default PreferenceSettings
