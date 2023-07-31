'use client'

import type { layout_LibrarySlugPageLayoutQuery } from '@/__generated__/layout_LibrarySlugPageLayoutQuery.graphql'

import { notFound } from 'next/navigation'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { LibraryProvider } from '@/app/(libraries)/library/[slug]/use-library.context'
import { useLibrary } from '@/app/(libraries)/library/[slug]/use-library.hook'

type LibrarySlugPageLayoutProps = React.PropsWithChildren & {
  params: {
    slug: string
  }
}

const LibrarySlugPageLayout: React.FC<LibrarySlugPageLayoutProps> = ({ children, params }) => {
  const query = useLazyLoadQuery<layout_LibrarySlugPageLayoutQuery>(
    graphql`
      query layout_LibrarySlugPageLayoutQuery($where: LibraryFilterInput) {
        library(where: $where) {
          ...useLibraryFragment
        }
      }
    `,
    {
      where: {
        slug: {
          eq: params.slug,
        },
      },
    }
  )

  if (query.library == null) {
    notFound()
  }

  const context = useLibrary({ $key: query.library })

  return <LibraryProvider value={context}>{children}</LibraryProvider>
}

export default LibrarySlugPageLayout
