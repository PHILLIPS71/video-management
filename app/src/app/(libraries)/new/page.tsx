'use client'

import type { LibraryCreatePayload } from '@/components/forms/library'
import type { LibraryCreateRef } from '@/components/forms/library/LibraryCreate'

import { Button, Card, Typography } from '@giantnodes/react'
import { useRouter } from 'next/navigation'
import React from 'react'

import { LibraryCreate } from '@/components/forms/library'

const LibraryCreatePage = () => {
  const router = useRouter()
  const ref = React.useRef<LibraryCreateRef>(null)

  const onComplete = (payload: LibraryCreatePayload) => {
    router.push(`/library/${payload.slug}/explore`)
  }

  return (
    <section className="mx-auto max-w-3xl">
      <Card>
        <Card.Header>
          <Typography.Paragraph className="font-semibold text-md">Create a new library</Typography.Paragraph>
          <Typography.Paragraph variant="subtitle">
            Your library will have its own dedicated metrics and control panel. A dashboard will be set up so you can
            easily interact with your new library.
          </Typography.Paragraph>
        </Card.Header>

        <Card.Body>
          <LibraryCreate ref={ref} onComplete={onComplete} />
        </Card.Body>

        <Card.Footer className="flex items-center justify-end gap-2">
          <div className="flex gap-2">
            <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
              Cancel
            </Button>
            <Button size="xs" onPress={() => ref.current?.submit()}>
              Save
            </Button>
          </div>
        </Card.Footer>
      </Card>
    </section>
  )
}

export default LibraryCreatePage
