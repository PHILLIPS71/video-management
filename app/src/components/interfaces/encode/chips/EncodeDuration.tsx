import type { EncodeDurationFragment$key } from '@/__generated__/EncodeDurationFragment.graphql'

import { Chip } from '@giantnodes/react'
import dayjs from 'dayjs'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodeDurationFragment on Encode {
    status
    failed_at
    cancelled_at
    completed_at
    created_at
  }
`

type EncodeDurationChipProps = {
  $key: EncodeDurationFragment$key
}

const EncodeDuration: React.FC<EncodeDurationChipProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  const date = React.useMemo<Date | undefined>(() => {
    switch (data.status) {
      case 'COMPLETED':
        return data.completed_at
      case 'CANCELLED':
        return data.cancelled_at

      case 'FAILED':
        return data.failed_at

      default:
        return undefined
    }
  }, [data.cancelled_at, data.completed_at, data.failed_at, data.status])

  return (
    <Chip color="indigo" title={dayjs(date).format('L LT')}>
      {dayjs.duration(dayjs(date).diff(data.created_at)).format('H[h] m[m] s[s]')}
    </Chip>
  )
}

export default EncodeDuration
