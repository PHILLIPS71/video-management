import type { useLibraryFragment$data, useLibraryFragment$key } from '@/__generated__/useLibraryFragment.graphql'

import React from 'react'
import { graphql, useFragment } from 'react-relay'

type UseLibraryProps = {
  $key: useLibraryFragment$key
}

export const useLibrary = ({ $key }: UseLibraryProps) => {
  const fragment = useFragment<useLibraryFragment$key>(
    graphql`
      fragment useLibraryFragment on Library {
        id
        slug
        name
        path_info {
          full_name
        }
      }
    `,
    $key
  )

  const library = React.useMemo<useLibraryFragment$data>(() => fragment, [fragment])

  return {
    library,
  }
}

export type UseLibraryReturn = ReturnType<typeof useLibrary>
