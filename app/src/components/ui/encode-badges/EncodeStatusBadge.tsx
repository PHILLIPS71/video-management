import type { EncodeStatus, EncodeStatusBadgeFragment$key } from '@/__generated__/EncodeStatusBadgeFragment.graphql'

import { Chip } from '@giantnodes/react'
import { graphql, useFragment } from 'react-relay'

type EncodeStatusBadgeProps = {
  $key: EncodeStatusBadgeFragment$key
}

const FRAGMENT = graphql`
  fragment EncodeStatusBadgeFragment on Encode {
    status
  }
`

const EncodeStatusBadge: React.FC<EncodeStatusBadgeProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  const color = (status: EncodeStatus) => {
    switch (status) {
      case 'SUBMITTED':
        return 'info'

      case 'QUEUED':
        return 'info'

      case 'ENCODING':
        return 'success'

      case 'DEGRADED':
        return 'warning'

      case 'COMPLETED':
        return 'success'

      case 'CANCELLED':
        return 'neutral'

      case 'FAILED':
        return 'danger'

      default:
        return 'neutral'
    }
  }

  return <Chip color={color(data.status)}>{data.status.toLowerCase()}</Chip>
}

export default EncodeStatusBadge
