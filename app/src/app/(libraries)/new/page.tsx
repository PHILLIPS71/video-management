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
  const [isLoading, setLoading] = React.useState<boolean>(false)

  const onComplete = (payload: LibraryCreatePayload) => {
    router.push(`/library/${payload.slug}/explore`)
  }

  return (
    <section className="mx-auto max-w-3xl">
      <Card>
        <Card.Header>
          <Typography.Paragraph>Create a new library</Typography.Paragraph>
          <Typography.Text variant="subtitle">
            Your library will have its own dedicated metrics and control panel. A dashboard will be set up so you can
            easily interact with your new library.
          </Typography.Text>
        </Card.Header>

        <Card.Body>
          <LibraryCreate ref={ref} onComplete={onComplete} onLoadingChange={setLoading} />
        </Card.Body>

        <Card.Footer className="flex items-center justify-end gap-3">
          <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
            Reset
          </Button>
          <Button isDisabled={isLoading} size="xs" onPress={() => ref.current?.submit()}>
            Save
          </Button>
        </Card.Footer>
      </Card>
    </section>
  )
}

export default LibraryCreatePage
