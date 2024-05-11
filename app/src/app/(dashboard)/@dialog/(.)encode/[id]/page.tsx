'use client'

import type { page_EncodedDialog_Query } from '@/__generated__/page_EncodedDialog_Query.graphql'

import { notFound, useRouter } from 'next/navigation'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { EncodeDialog } from '@/components/interfaces/dashboard'

const QUERY = graphql`
  query page_EncodedDialog_Query($where: EncodeFilterInput) {
    encode(where: $where) {
      ...EncodeDialogFragment
    }
  }
`

type EncodePageProps = React.PropsWithChildren & {
  params: {
    id: string
  }
}

const EncodePage: React.FC<EncodePageProps> = ({ children, params }) => {
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
      {children}
    </EncodeDialog>
  )
}

export default EncodePage
