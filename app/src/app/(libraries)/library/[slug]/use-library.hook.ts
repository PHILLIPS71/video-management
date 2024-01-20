import type { useLibraryFragment$data, useLibraryFragment$key } from '@/__generated__/useLibraryFragment.graphql'

import React from 'react'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment useLibraryFragment on Library {
    id
    slug
    name
    path_info {
      full_name
      directory_separator_char
    }
  }
`

type UseLibraryProps = {
  $key: useLibraryFragment$key
}

export const useLibrary = ({ $key }: UseLibraryProps) => {
  const fragment = useFragment<useLibraryFragment$key>(FRAGMENT, $key)

  const library = React.useMemo<useLibraryFragment$data>(() => fragment, [fragment])

  return {
    library,
  }
}

export type UseLibraryReturn = ReturnType<typeof useLibrary>
