'use client'

import type { provider_LibraryProviderQuery } from '@/__generated__/provider_LibraryProviderQuery.graphql'

import { notFound } from 'next/navigation'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { LibraryContext, useLibrary } from '@/app/(libraries)/library/[slug]/use-library.hook'

const QUERY = graphql`
  query provider_LibraryProviderQuery($where: LibraryFilterInput) {
    library(where: $where) {
      ...useLibraryFragment
    }
  }
`

type LibraryProviderProps = React.PropsWithChildren & {
  slug: string
}

const LibraryProvider: React.FC<LibraryProviderProps> = ({ children, slug }) => {
  const query = useLazyLoadQuery<provider_LibraryProviderQuery>(QUERY, {
    where: {
      slug: {
        eq: slug,
      },
    },
  })

  if (query.library == null) {
    notFound()
  }

  const context = useLibrary({ $key: query.library })

  return <LibraryContext.Provider value={context}>{children}</LibraryContext.Provider>
}

export default LibraryProvider
