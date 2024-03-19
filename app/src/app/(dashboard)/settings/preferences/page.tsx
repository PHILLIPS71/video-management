'use client'

import type { PreferenceSettingsRef } from '@/components/forms'
import type { NextPage } from 'next'

import { Button, Card, Typography } from '@giantnodes/react'
import React from 'react'

import { PreferenceSettings } from '@/components/forms'

const PreferenceSettingsPage: NextPage = () => {
  const ref = React.useRef<PreferenceSettingsRef>(null)

  return (
    <section className="max-w-5xl">
      <Card>
        <Card.Header>
          <Typography.Paragraph className="font-semibold text-md">Theme</Typography.Paragraph>
        </Card.Header>

        <Card.Body>
          <PreferenceSettings ref={ref} />
        </Card.Body>

        <Card.Footer className="flex items-center justify-end gap-3">
          <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
            Cancel
          </Button>
          <Button size="xs" onPress={() => ref.current?.submit()}>
            Save
          </Button>
        </Card.Footer>
      </Card>
    </section>
  )
}

export default PreferenceSettingsPage
