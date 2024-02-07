'use client'

import type { LibraryUpdateRef } from '@/components/forms/library'

import { Button, Card, Typography } from '@giantnodes/react'
import React from 'react'

import { useLibraryContext } from '@/app/(libraries)/library/[slug]/use-library.hook'
import { LibraryUpdate } from '@/components/forms/library'

const LibrarySettingsGeneralPage = () => {
  const { library } = useLibraryContext()

  const ref = React.useRef<LibraryUpdateRef>(null)
  const [isLoading, setLoading] = React.useState<boolean>(false)

  return (
    <section className="max-w-5xl">
      <Card>
        <Card.Header>
          <Typography.Paragraph className="font-semibold text-md">General</Typography.Paragraph>
        </Card.Header>

        <Card.Body>
          <LibraryUpdate ref={ref} library={library} onLoadingChange={setLoading} />
        </Card.Body>

        <Card.Footer className="flex items-center justify-end gap-2">
          <div className="flex gap-2">
            <Button color="neutral" size="xs" onPress={() => ref.current?.reset()}>
              Cancel
            </Button>
            <Button isDisabled={isLoading} size="xs" onPress={() => ref.current?.submit()}>
              Save
            </Button>
          </div>
        </Card.Footer>
      </Card>
    </section>
  )
}

export default LibrarySettingsGeneralPage
