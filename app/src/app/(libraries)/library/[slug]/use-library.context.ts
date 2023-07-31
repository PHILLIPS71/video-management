'use client'

import type { UseLibraryReturn } from '@/app/(libraries)/library/[slug]/use-library.hook'

import { createContext } from '@/utilities/context'

export const [LibraryProvider, useLibraryContext] = createContext<UseLibraryReturn>({
  name: 'LibraryProvider',
  strict: true,
  errorMessage:
    'useLibraryProvider: `context` is undefined. Seems you forgot to wrap component within <LibraryProvider />',
})
