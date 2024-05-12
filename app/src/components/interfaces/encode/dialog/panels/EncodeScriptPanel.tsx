import type { EncodeScriptPanelFragment$key } from '@/__generated__/EncodeScriptPanelFragment.graphql'

import { Alert, Card, Typography } from '@giantnodes/react'
import { IconAlertCircleFilled } from '@tabler/icons-react'
import { graphql, useFragment } from 'react-relay'

import { EncodeCommandWidget, EncodeOperationWidget, EncodeOutputWidget } from '@/components/interfaces/encode'

const FRAGMENT = graphql`
  fragment EncodeScriptPanelFragment on Encode {
    failure_reason
    ...EncodeOperationWidgetFragment
    ...EncodeCommandWidgetFragment
    ...EncodeOutputWidgetFragment
  }
`

type EncodeScriptPanelProps = {
  $key: EncodeScriptPanelFragment$key
}

const EncodeScriptPanel: React.FC<EncodeScriptPanelProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return (
    <>
      {data.failure_reason && (
        <Alert color="danger">
          <IconAlertCircleFilled size={16} />
          <Alert.Body>
            <Alert.Heading>The encode operation encountered an error</Alert.Heading>
            <Alert.List>
              <Alert.Item>{data.failure_reason}</Alert.Item>
            </Alert.List>
          </Alert.Body>
        </Alert>
      )}

      <Card className="flex-none">
        <EncodeOperationWidget $key={data} />
      </Card>

      <Card className="flex-none">
        <Card.Header>
          <Typography.Text>Command</Typography.Text>
        </Card.Header>

        <Card.Body>
          <EncodeCommandWidget $key={data} />
        </Card.Body>
      </Card>

      <Card className="shrink">
        <Card.Header>
          <Typography.Text>Output</Typography.Text>
        </Card.Header>

        <Card.Body className="overflow-y-auto">
          <EncodeOutputWidget $key={data} />
        </Card.Body>
      </Card>
    </>
  )
}

export default EncodeScriptPanel
