import type { EncodeDialogScriptFragment$key } from '@/__generated__/EncodeDialogScriptFragment.graphql'

import { Card } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

import EncodeDialogCommand from '@/components/interfaces/dashboard/encode-dialog/EncodeDialogCommand'
import EncodeDialogLog from '@/components/interfaces/dashboard/encode-dialog/EncodeDialogLog'

const FRAGMENT = graphql`
  fragment EncodeDialogScriptFragment on Encode {
    ...EncodeDialogCommandFragment
    ...EncodeDialogLogFragment
  }
`

type EncodeDialogScriptProps = {
  $key: EncodeDialogScriptFragment$key
}

const EncodeDialogScript: React.FC<EncodeDialogScriptProps> = ({ $key }) => {
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
        <Card.Header>Logs</Card.Header>

        <Card.Body className="overflow-y-auto">
          <EncodeDialogLog $key={data} />
        </Card.Body>
      </Card>
    </>
  )
}

export default EncodeDialogScript
