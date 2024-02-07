import type { useLibraryFragment$data, useLibraryFragment$key } from '@/__generated__/useLibraryFragment.graphql'

import React from 'react'
import { graphql, useFragment } from 'react-relay'

import { createContext } from '@/utilities/context'

const FRAGMENT = graphql`
  fragment useLibraryFragment on Library {
    id
    name
    slug
    is_watched
    path_info {
      full_name
      directory_separator_char
    }
  }
`

type UseLibraryProps = {
  $key: useLibraryFragment$key
}

type UseLibraryReturn = ReturnType<typeof useLibrary>

export const useLibrary = ({ $key }: UseLibraryProps) => {
  const fragment = useFragment<useLibraryFragment$key>(FRAGMENT, $key)

  const library = React.useMemo<useLibraryFragment$data>(() => fragment, [fragment])

  return {
    library,
  }
}

export const [LibraryContext, useLibraryContext] = createContext<UseLibraryReturn>({
  name: 'LibraryContext',
  strict: true,
  errorMessage:
    'useLibraryContext: `context` is undefined. Seems you forgot to wrap component within <LibraryContext.Provider />',
})
