import type { EncodeStatusFragment$key } from '@/__generated__/EncodeStatusFragment.graphql'
import type { ChipProps } from '@giantnodes/react'

import { Chip } from '@giantnodes/react'
import dayjs from 'dayjs'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodeStatusFragment on Encode {
    status
    failed_at
    cancelled_at
    completed_at
  }
`

type EncodeStatusChipProps = {
  $key: EncodeStatusFragment$key
}

const EncodeStatus: React.FC<EncodeStatusChipProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  const color = React.useMemo<ChipProps['color']>(() => {
    switch (data.status) {
      case 'COMPLETED':
        return 'brand'

      case 'CANCELLED':
        return 'neutral'

      case 'FAILED':
        return 'danger'

      default:
        return 'warning'
    }
  }, [data.status])

  const title = React.useMemo<string | undefined>(() => {
    switch (data.status) {
      case 'COMPLETED':
        return dayjs(data.completed_at).format('L LTS')

      case 'CANCELLED':
        return dayjs(data.cancelled_at).format('L LTS')

      case 'FAILED':
        return dayjs(data.failed_at).format('L LTS')

      default:
        return undefined
    }
  }, [data.cancelled_at, data.completed_at, data.failed_at, data.status])

  return (
    <Chip color={color} title={title}>
      {data.status.toLowerCase()}
    </Chip>
  )
}

export default EncodeStatus
