import React from 'react'

import { createContext } from '@/utilities/context'

export enum EncodeDialogPage {
  SCRIPT,
  ANALYTICS,
}

type UseEncodeDialogReturn = ReturnType<typeof useEncodeDialog>

type UseEncodeDialogProps = {
  page: EncodeDialogPage
}

export const useEncodeDialog = (props: UseEncodeDialogProps) => {
  const [page, setPage] = React.useState<EncodeDialogPage>(props.page)

  return {
    page,
    setPage,
  }
}

export const [EncodeDialogContext, useEncodeDialogContext] = createContext<UseEncodeDialogReturn>({
  name: 'EncodeDialogContext',
  strict: true,
  errorMessage:
    'useEncodeDialogContext: `context` is undefined. Seems you forgot to wrap component within <EncodeDialogContext.Provider />',
})
