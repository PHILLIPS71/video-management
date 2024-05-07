import type { EncodeDialogFragment$key } from '@/__generated__/EncodeDialogFragment.graphql'

import { Button, Card, Chip, Dialog, Typography } from '@giantnodes/react'
import { IconX } from '@tabler/icons-react'
import React from 'react'
import { graphql, useFragment, useSubscription } from 'react-relay'

import EncodeDialogScript from '@/components/interfaces/dashboard/encode-dialog/EncodeDialogScript'
import EncodeDialogSidebar from '@/components/interfaces/dashboard/encode-dialog/EncodeDialogSidebar'
import {
  EncodeDialogContext,
  EncodeDialogPage,
  useEncodeDialog,
} from '@/components/interfaces/dashboard/encode-dialog/use-encode-dialog.hook'
import EncodeStatusBadge from '@/components/ui/encode-badges/EncodeStatusBadge'

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
    ...EncodeStatusBadgeFragment
    ...EncodeDialogScriptFragment
  }
`

const OUTPUTTED_SUBSCRIPTION = graphql`
  subscription EncodeDialogOutputtedSubscription {
    encode_outputted {
      output
    }
  }
`

type EncodeDialogProps = React.PropsWithChildren & {
  $key: EncodeDialogFragment$key
}

const EncodeDialog: React.FC<EncodeDialogProps> = ({ children, $key }) => {
  const data = useFragment(FRAGMENT, $key)
  const context = useEncodeDialog({ page: EncodeDialogPage.SCRIPT })

  useSubscription({
    subscription: OUTPUTTED_SUBSCRIPTION,
    variables: {},
  })

  const content = React.useCallback(() => {
    switch (context.page) {
      case EncodeDialogPage.SCRIPT:
        return <EncodeDialogScript $key={data} />

      case EncodeDialogPage.ANALYTICS:
        return <EncodeDialogScript $key={data} />

      default:
        throw new Error(`unexpected value ${context.page} was provided.`)
    }
  }, [context.page, data])

  return (
    <Dialog placement="right">
      {children}

      <Dialog.Content>
        {({ close }) => (
          <EncodeDialogContext.Provider value={context}>
            <Card className="flex flex-row overflow-hidden">
              <EncodeDialogSidebar />

              <Card className="bg-background">
                <Card.Header>
                  <div className="flex flex-row gap-3 justify-between">
                    <div className="flex flex-row items-center flex-wrap gap-3">
                      <Typography.Paragraph>{data.file.path_info.name}</Typography.Paragraph>

                      <EncodeStatusBadge $key={data} />

                      <Chip color="info">{data.recipe.name}</Chip>
                    </div>

                    <div className="flex items-center gap-3">
                      <Button color="transparent" size="xs" onPress={close}>
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
