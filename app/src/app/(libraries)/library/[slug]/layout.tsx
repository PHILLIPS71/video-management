'use client'

import type { layout_LibrarySlugPageLayoutQuery } from '@/__generated__/layout_LibrarySlugPageLayoutQuery.graphql'

import { notFound } from 'next/navigation'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { LibraryContext, useLibrary } from '@/app/(libraries)/library/[slug]/use-library.hook'
import LibraryLayout from '@/components/layouts/library/LibraryLayout'

const QUERY = graphql`
  query layout_LibrarySlugPageLayoutQuery($where: LibraryFilterInput) {
    library(where: $where) {
      ...useLibraryFragment
    }
  }
`

type LibrarySlugPageLayoutProps = React.PropsWithChildren & {
  params: {
    slug: string
  }
}

const LibrarySlugPageLayout: React.FC<LibrarySlugPageLayoutProps> = ({ children, params }) => {
  const query = useLazyLoadQuery<layout_LibrarySlugPageLayoutQuery>(QUERY, {
    where: {
      slug: {
        eq: params.slug,
      },
    },
  })

  if (query.library == null) {
    notFound()
  }

  const context = useLibrary({ $key: query.library })

  return (
    <LibraryContext.Provider value={context}>
      <LibraryLayout>{children}</LibraryLayout>
    </LibraryContext.Provider>
  )
}

export const dynamic = 'force-dynamic'
export default LibrarySlugPageLayout
