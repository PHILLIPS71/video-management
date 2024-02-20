import type { EncodeBadgesFragment$key, EncodeStatus } from '@/__generated__/EncodeBadgesFragment.graphql'
import type { ChipProps } from '@giantnodes/react'

import { Chip } from '@giantnodes/react'
import { IconArrowRight, IconTrendingDown, IconTrendingUp } from '@tabler/icons-react'
import dayjs from 'dayjs'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

type EncodeBadgesProps = Omit<ChipProps, 'color'> & {
  $key: EncodeBadgesFragment$key
}

const FRAGMENT = graphql`
  fragment EncodeBadgesFragment on Encode {
    status
    percent
    started_at
    failed_at
    cancelled_at
    completed_at
    created_at
    speed {
      frames
      bitrate
      scale
    }
    snapshots {
      size
      probed_at
    }
  }
`

const EncodeBadges: React.FC<EncodeBadgesProps> = ({ $key, radius, size, variant }) => {
  const data = useFragment(FRAGMENT, $key)

  const chipProps = React.useMemo<ChipProps>(() => ({ radius, size, variant }), [radius, size, variant])

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

  const getStatusColour = (status: EncodeStatus) => {
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

  const SizeChip = React.useCallback(() => {
    const difference = data.snapshots[data.snapshots.length - 1].size - data.snapshots[0].size
    const increase = Math.abs(difference / data.snapshots[0].size)

    const icon = () => {
      switch (true) {
        case increase > 0:
          return <IconTrendingUp size={14} />

        case increase < 0:
          return <IconTrendingDown size={14} />

        case increase === 0:
        default:
          return <IconArrowRight size={14} />
      }
    }

    const color = () => {
      switch (true) {
        case increase > 0:
          return 'danger'

        case increase < 0:
          return 'success'

        case increase === 0:
        default:
          return 'info'
      }
    }

    return (
      <Chip color={color()} {...chipProps}>
        {icon()}

        {percent(increase)}
      </Chip>
    )
  }, [chipProps, data.snapshots])

  return (
    <div className="flex flex-row items-center justify-end gap-2">
      <Chip color={getStatusColour(data.status)}>{data.status.toLowerCase()}</Chip>

      {data.status !== 'COMPLETED' && data.status !== 'CANCELLED' && data.status !== 'FAILED' && (
        <>
          {data.speed != null && (
            <>
              <Chip color="info" {...chipProps}>
                {data.speed.frames} fps
              </Chip>

              <Chip color="info" {...chipProps}>
                {filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s
              </Chip>

              <Chip color="info" {...chipProps}>
                {data.speed.scale.toFixed(2)}x
              </Chip>
            </>
          )}

          {data.percent != null && (
            <Chip color="info" {...chipProps}>
              {percent(data.percent)}
            </Chip>
          )}
        </>
      )}

      {data.status === 'COMPLETED' && (
        <>
          <Chip color="info" {...chipProps}>
            {dayjs
              .duration(dayjs(data.completed_at ?? data.failed_at).diff(data.started_at ?? data.created_at))
              .format('H[h] m[m] s[s]')}
          </Chip>

          {SizeChip()}
        </>
      )}

      {data.status === 'CANCELLED' && (
        <Chip color="info" {...chipProps}>
          {dayjs.duration(dayjs(data.cancelled_at).diff(data.started_at ?? data.created_at)).format('H[h] m[m] s[s]')}
        </Chip>
      )}

      {data.status === 'FAILED' && (
        <Chip color="info" {...chipProps}>
          {dayjs.duration(dayjs(data.failed_at).diff(data.started_at ?? data.created_at)).format('H[h] m[m] s[s]')}
        </Chip>
      )}
    </div>
  )
}

export default EncodeBadges
