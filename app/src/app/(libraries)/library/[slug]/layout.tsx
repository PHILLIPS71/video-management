import type { Metadata } from 'next'

import React from 'react'

import LibraryProvider from '@/app/(libraries)/library/[slug]/provider'
import LibraryLayout from '@/components/layouts/library/LibraryLayout'

type LibraryLayoutProps = React.PropsWithChildren & {
  params: {
    slug: string
  }
}

export async function generateMetadata({ params }: LibraryLayoutProps): Promise<Metadata> {
  return {
    title: params.slug.replaceAll('-', ' '),
  }
}

const LibrarySlugLayout: React.FC<LibraryLayoutProps> = ({ children, params }) => (
  <LibraryProvider slug={params.slug}>
    <LibraryLayout>{children}</LibraryLayout>
  </LibraryProvider>
)

export default LibrarySlugLayout
