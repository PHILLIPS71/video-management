import type { EncodeDialogFragment$key } from '@/__generated__/EncodeDialogFragment.graphql'
import type { DialogProps } from '@giantnodes/react'

import { Button, Card, Chip, Dialog, Typography } from '@giantnodes/react'
import { IconX } from '@tabler/icons-react'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import EncodeDialogSidebar from '@/components/interfaces/encode/dialog/EncodeDialogSidebar'
import EncodeAnalyticsPanel from '@/components/interfaces/encode/dialog/panels/EncodeAnalyticsPanel'
import EncodeDialogScript from '@/components/interfaces/encode/dialog/panels/EncodeScriptPanel'
import {
  EncodeDialogContext,
  EncodeDialogPanel,
  useEncodeDialog,
} from '@/components/interfaces/encode/dialog/use-encode-dialog.hook'

const FRAGMENT = graphql`
  fragment EncodeDialogFragment on Encode {
    recipe {
      name
    }
    file {
      path_info {
        name
      }
    }
    ...EncodeScriptPanelFragment
    ...EncodeAnalyticsPanelFragment
  }
`

type EncodeDialogProps = React.PropsWithChildren & {
  $key: EncodeDialogFragment$key
} & DialogProps

const EncodeDialog: React.FC<EncodeDialogProps> = ({ $key, children, ...rest }) => {
  const data = useFragment(FRAGMENT, $key)
  const context = useEncodeDialog({ panel: EncodeDialogPanel.SCRIPT })

  const content = React.useCallback(() => {
    switch (context.panel) {
      case EncodeDialogPanel.SCRIPT:
        return <EncodeDialogScript $key={data} />

      case EncodeDialogPanel.ANALYTICS:
        return <EncodeAnalyticsPanel $key={data} />

      default:
        throw new Error(`unexpected panel value ${context.panel} was provided.`)
    }
  }, [context.panel, data])

  return (
    <Dialog placement="right" {...rest}>
      {children}

      <Dialog.Content>
        {({ close }) => (
          <EncodeDialogContext.Provider value={context}>
            <Card className="flex flex-row overflow-hidden">
              <EncodeDialogSidebar />

              <Card className="grow bg-background">
                <Card.Header>
                  <div className="flex flex-row gap-3 justify-between">
                    <div className="flex flex-row items-center flex-wrap gap-3">
                      <Typography.Paragraph>{data.file.path_info.name}</Typography.Paragraph>

                      <Chip color="info">{data.recipe.name}</Chip>
                    </div>

                    <div className="flex items-center gap-3">
                      <Button
                        color="transparent"
                        size="xs"
                        onPress={() => {
                          close()
                          context.setPanel(EncodeDialogPanel.SCRIPT)
                        }}
                      >
                        <IconX size={16} strokeWidth={1} />
                      </Button>
                    </div>
                  </div>
                </Card.Header>

                <Card.Body className="overflow-hidden">
                  <div className="flex flex-col grow gap-3 h-full">{content()}</div>
                </Card.Body>
              </Card>
            </Card>
          </EncodeDialogContext.Provider>
        )}
      </Dialog.Content>
    </Dialog>
  )
}

export default EncodeDialog
