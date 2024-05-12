'use client'

import type { page_EncodedPage_Query } from '@/__generated__/page_EncodedPage_Query.graphql'

import { Alert, Card, Typography } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import { notFound } from 'next/navigation'
import { graphql, useLazyLoadQuery } from 'react-relay'

import {
  EncodeCommandWidget,
  EncodeOperationWidget,
  EncodeOutputWidget,
  EncodeSizeWidget,
} from '@/components/interfaces/encode'
import { FileSystemBreadcrumb } from '@/components/interfaces/file-system'

const QUERY = graphql`
  query page_EncodedPage_Query($where: EncodeFilterInput) {
    encode(where: $where) {
      failure_reason
      file {
        ...FileSystemBreadcrumbFragment
      }
      ...EncodeOperationWidgetFragment
      ...EncodeCommandWidgetFragment
      ...EncodeOutputWidgetFragment
      ...EncodeSizeWidgetFragment
    }
  }
`

type EncodePageProps = {
  params: {
    [x: string]: never
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
          {query.encode.failure_reason && (
            <Alert color="danger">
              <IconAlertCircleFilled size={16} />
              <Alert.Body>
                <Alert.Heading>The encode operation encountered an error</Alert.Heading>
                <Alert.List>
                  <Alert.Item>{query.encode.failure_reason}</Alert.Item>
                </Alert.List>
              </Alert.Body>
            </Alert>
          )}

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
            <EncodeOperationWidget $key={query.encode} />
          </Card>
        </div>
      </div>
    </div>
  )
}

export default EncodePage
