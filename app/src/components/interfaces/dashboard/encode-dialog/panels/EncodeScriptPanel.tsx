import type { EncodeScriptPanelFragment$key } from '@/__generated__/EncodeScriptPanelFragment.graphql'

import { Card } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

import EncodeDialogCommand from '@/components/interfaces/dashboard/encode-dialog/widgets/EncodeCommandWidget'
import EncodeDialogLog from '@/components/interfaces/dashboard/encode-dialog/widgets/EncodeOutputWidget'

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
        <Card.Header>Command</Card.Header>

        <Card.Body>
          <EncodeDialogCommand $key={data} />
        </Card.Body>
      </Card>

      <Card className="shrink">
        <Card.Header>Output</Card.Header>

        <Card.Body className="overflow-y-auto">
          <EncodeDialogLog $key={data} />
        </Card.Body>
      </Card>
    </>
  )
}

export default EncodeScriptPanel
