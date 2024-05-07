import type { EncodeBadgesFragment$key } from '@/__generated__/EncodeBadgesFragment.graphql'
import type { ChipProps } from '@giantnodes/react'

import { Chip } from '@giantnodes/react'
import { IconArrowRight, IconTrendingDown, IconTrendingUp } from '@tabler/icons-react'
import dayjs from 'dayjs'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import EncodeStatusBadge from '@/components/ui/encode-badges/EncodeStatusBadge'

type EncodeBadgesProps = Omit<ChipProps, 'color'> & {
  $key: EncodeBadgesFragment$key
}

const FRAGMENT = graphql`
  fragment EncodeBadgesFragment on Encode {
    status
    percent
    started_at
    failure_reason
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
    ...EncodeStatusBadgeFragment
  }
`

const EncodeBadges: React.FC<EncodeBadgesProps> = ({ $key, size }) => {
  const data = useFragment(FRAGMENT, $key)

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

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
      <Chip color={color()} size={size}>
        {icon()}

        {percent(increase)}
      </Chip>
    )
  }, [data.snapshots, size])

  return (
    <div className="flex flex-row items-center justify-end gap-2">
      <EncodeStatusBadge $key={data} />

      {data.status !== 'COMPLETED' && data.status !== 'CANCELLED' && data.status !== 'FAILED' && (
        <>
          {data.speed != null && (
            <>
              <Chip color="info" size={size}>
                {data.speed.frames} fps
              </Chip>

              <Chip color="info" size={size}>
                {filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s
              </Chip>

              <Chip color="info" size={size}>
                {data.speed.scale.toFixed(2)}x
              </Chip>
            </>
          )}

          {data.percent != null && (
            <Chip color="info" size={size}>
              {percent(data.percent)}
            </Chip>
          )}
        </>
      )}

      {data.status === 'COMPLETED' && (
        <>
          <Chip color="info" size={size} title={dayjs(data.completed_at).format('L LT')}>
            {dayjs.duration(dayjs(data.completed_at).diff(data.created_at)).format('H[h] m[m] s[s]')}
          </Chip>

          {SizeChip()}
        </>
      )}

      {data.status === 'CANCELLED' && (
        <Chip color="info" size={size} title={dayjs(data.cancelled_at).format('L LT')}>
          {dayjs.duration(dayjs(data.cancelled_at).diff(data.created_at)).format('H[h] m[m] s[s]')}
        </Chip>
      )}

      {data.status === 'FAILED' && (
        <Chip color="info" size={size} title={dayjs(data.failed_at).format('L LT')}>
          {dayjs.duration(dayjs(data.failed_at).diff(data.created_at)).format('H[h] m[m] s[s]')}
        </Chip>
      )}
    </div>
  )
}

export default EncodeBadges
