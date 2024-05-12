'use client'

import type { page_EncodedDialog_Query } from '@/__generated__/page_EncodedDialog_Query.graphql'

import { notFound, useRouter } from 'next/navigation'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeDialog } from '@/components/interfaces/encode'

const QUERY = graphql`
  query page_EncodedDialog_Query($where: EncodeFilterInput) {
    encode(where: $where) {
      ...EncodeDialogFragment
    }
  }
`

type EncodePageProps = {
  params: {
    [x: string]: never
  }
}

const EncodePage: React.FC<EncodePageProps> = ({ params }) => {
  const router = useRouter()

  const query = useLazyLoadQuery<page_EncodedDialog_Query>(QUERY, {
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
    <EncodeDialog isOpen $key={query.encode} onOpenChange={router.back}>
      Open
    </EncodeDialog>
  )
}

export default EncodePage
