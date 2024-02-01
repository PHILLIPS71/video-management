import type { Selection } from '@giantnodes/react'

import React from 'react'

import { createContext } from '@/utilities/context'

type UseExploreReturn = ReturnType<typeof useExplore>

type UseExploreProps = {
  directory: string
}

export const useExplore = (props: UseExploreProps) => {
  const { directory } = props

  const [keys, setKeys] = React.useState<Selection>(new Set<string>())

  return {
    directory,

    keys,
    setKeys,
  }
}

export const [ExploreContext, useExploreContext] = createContext<UseExploreReturn>({
  name: 'ExploreContext',
  strict: true,
  errorMessage:
    'useExploreContext: `context` is undefined. Seems you forgot to wrap component within <ExploreContext.Provider />',
})
