import type { EncodeDialogFragment$key } from '@/__generated__/EncodeDialogFragment.graphql'

import { Button, Card, Chip, Dialog, Typography } from '@giantnodes/react'
import { IconX } from '@tabler/icons-react'
import { graphql, useFragment } from 'react-relay'

import CodeBlock from '@/components/ui/code-block/CodeBlock'
import EncodeStatusBadge from '@/components/ui/encode-badges/EncodeStatusBadge'

const FRAGMENT = graphql`
  fragment EncodeDialogFragment on Encode {
    command
    output
    recipe {
      name
    }
    file {
      path_info {
        name
      }
    }
    ...EncodeStatusBadgeFragment
  }
`

type EncodeDialogProps = React.PropsWithChildren & {
  $key: EncodeDialogFragment$key
}

const EncodeDialog: React.FC<EncodeDialogProps> = ({ children, $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return (
    <Dialog placement="right">
      {children}

      <Dialog.Content>
        {({ close }) => (
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
              <div className="flex flex-col grow gap-3 h-full">
                {data.command && (
                  <Card className="flex-none">
                    <Card.Header>Command</Card.Header>

                    <Card.Body>
                      <CodeBlock language="powershell">{data.command}</CodeBlock>
                    </Card.Body>
                  </Card>
                )}

                {data.output && (
                  <Card className="shrink">
                    <Card.Header>Logs</Card.Header>

                    <Card.Body className="overflow-y-auto">
                      <CodeBlock language="excel">{data.output}</CodeBlock>
                    </Card.Body>
                  </Card>
                )}
              </div>
            </Card.Body>
          </Card>
        )}
      </Dialog.Content>
    </Dialog>
  )
}

export default EncodeDialog
