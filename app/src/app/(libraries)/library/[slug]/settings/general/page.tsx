'use client'

import type { LibraryUpdatePayload, LibraryUpdateRef } from '@/components/forms/library'

import { Button, Card, Typography } from '@giantnodes/react'
import { useRouter } from 'next/navigation'
import React from 'react'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'
import { LibraryUpdate } from '@/components/forms/library'

const LibrarySettingsGeneralPage = () => {
  const router = useRouter()
  const { library } = useLibraryContext()

  const ref = React.useRef<LibraryUpdateRef>(null)
  const [isLoading, setLoading] = React.useState<boolean>(false)

  const onComplete = (payload: LibraryUpdatePayload) => {
    if (library.slug !== payload.slug) {
      router.push(`/library/${payload.slug}/settings/general`)
    }
  }

  return (
    <section className="max-w-5xl">
      <Card>
        <Card.Header>
          <Typography.Paragraph className="font-semibold text-md">General</Typography.Paragraph>
        </Card.Header>

        <Card.Body>
          <LibraryUpdate ref={ref} library={library} onComplete={onComplete} onLoadingChange={setLoading} />
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

export default LibrarySettingsGeneralPage
