'use client'

import type { page_EncodedPage_Query } from '@/__generated__/page_EncodedPage_Query.graphql'

import { Card, Typography } from '@giantnodes/react'
import { notFound } from 'next/navigation'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeCommandWidget, EncodeOutputWidget, EncodeSizeWidget } from '@/components/interfaces/encode'
import { FileSystemBreadcrumb } from '@/components/interfaces/file-system'

const QUERY = graphql`
  query page_EncodedPage_Query($where: EncodeFilterInput) {
    encode(where: $where) {
      file {
        ...FileSystemBreadcrumbFragment
      }
      ...EncodeCommandWidgetFragment
      ...EncodeOutputWidgetFragment
      ...EncodeSizeWidgetFragment
    }
  }
`

type EncodePageProps = React.PropsWithChildren & {
  params: {
    id: string
  }
}

const EncodePage: React.FC<EncodePageProps> = ({ params }) => {
  const query = useLazyLoadQuery<page_EncodedPage_Query>(QUERY, {
    where: {
      id: {
        eq: decodeURIComponent(params.id),
      },
    },
  })

  if (query.encode == null) {
    return notFound()
  }

  return (
    <div className="max-w-6xl mx-auto ">
      <div className="flex flex-col lg:flex-row gap-3">
        <div className="flex flex-col grow gap-3">
          <Card>
            <Card.Body>
              <FileSystemBreadcrumb $key={query.encode.file} />
            </Card.Body>
          </Card>

          <Card>
            <Card.Header>
              <Typography.Text>Size</Typography.Text>
            </Card.Header>

            <Card.Body className="h-56">
              <EncodeSizeWidget $key={query.encode} />
            </Card.Body>
          </Card>

          <Card>
            <Card.Header>
              <Typography.Text>Command</Typography.Text>
            </Card.Header>

            <Card.Body>
              <EncodeCommandWidget $key={query.encode} />
            </Card.Body>
          </Card>

          <Card>
            <Card.Header>
              <Typography.Text>Output</Typography.Text>
            </Card.Header>

            <Card.Body>
              <EncodeOutputWidget $key={query.encode} />
            </Card.Body>
          </Card>
        </div>

        <div className="flex flex-col gap-3 min-w-80">
          <Card>
            <Card.Header>
              <Typography.Text>Tbd</Typography.Text>
            </Card.Header>
          </Card>
        </div>
      </div>
    </div>
  )
}

export default EncodePage
