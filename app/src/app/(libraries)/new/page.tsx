'use client'

import type { LibraryCreatePayload } from '@/components/forms/library-create/LibraryCreate'

import { Card, Typography } from '@giantnodes/react'
import { useRouter } from 'next/navigation'
import React from 'react'

import { LibraryCreate } from '@/components/forms'

const LibraryCreatePage = () => {
  const router = useRouter()

  const onComplete = (payload: LibraryCreatePayload) => {
    router.push(`/library/${payload.slug}/explore`)
  }

  return (
    <section className="mx-auto max-w-2xl">
      <Card>
        <Card.Header>
          <Typography.HeadingLevel>
            <Typography.Heading level={1}>Create a new library</Typography.Heading>
            <Typography.Paragraph>
              Your library will have its own dedicated metrics and control panel. A dashboard will be set up so you can
              easily interact with your new library.
            </Typography.Paragraph>
          </Typography.HeadingLevel>
        </Card.Header>

        <Card.Body>
          <LibraryCreate onComplete={onComplete} />
        </Card.Body>
      </Card>
    </section>
  )
}

export default LibraryCreatePage
