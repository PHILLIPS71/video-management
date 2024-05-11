import type { EncodeScriptPanelFragment$key } from '@/__generated__/EncodeScriptPanelFragment.graphql'

import { Card, Typography } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

import { EncodeCommandWidget, EncodeOutputWidget } from '@/components/interfaces/encode'

const FRAGMENT = graphql`
  fragment EncodeScriptPanelFragment on Encode {
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
