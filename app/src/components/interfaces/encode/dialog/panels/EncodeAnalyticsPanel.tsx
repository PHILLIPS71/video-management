import type { EncodeAnalyticsPanelFragment$key } from '@/__generated__/EncodeAnalyticsPanelFragment.graphql'

import { Card, Typography } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

import { EncodeSizeWidget } from '@/components/interfaces/encode'

const FRAGMENT = graphql`
  fragment EncodeAnalyticsPanelFragment on Encode {
    ...EncodeSizeWidgetFragment
  }
`

type EncodeAnalyticsPanelProps = {
  $key: EncodeAnalyticsPanelFragment$key
}

const EncodeAnalyticsPanel: React.FC<EncodeAnalyticsPanelProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return (
    <Card>
      <Card.Header>
        <Typography.Text>Size</Typography.Text>
      </Card.Header>

      <Card.Body className="h-56">
        <EncodeSizeWidget $key={data} />
      </Card.Body>
    </Card>
  )
}

export default EncodeAnalyticsPanel
