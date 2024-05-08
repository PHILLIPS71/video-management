import React from 'react'

import { createContext } from '@/utilities/context'

export enum EncodeDialogPanel {
  SCRIPT,
  ANALYTICS,
}

type UseEncodeDialogReturn = ReturnType<typeof useEncodeDialog>

type UseEncodeDialogProps = {
  panel: EncodeDialogPanel
}

export const useEncodeDialog = (props: UseEncodeDialogProps) => {
  const [panel, setPanel] = React.useState<EncodeDialogPanel>(props.panel)

  return {
    panel,
    setPanel,
  }
}

export const [EncodeDialogContext, useEncodeDialogContext] = createContext<UseEncodeDialogReturn>({
  name: 'EncodeDialogContext',
  strict: true,
  errorMessage:
    'useEncodeDialogContext: `context` is undefined. Seems you forgot to wrap component within <EncodeDialogContext.Provider />',
})
