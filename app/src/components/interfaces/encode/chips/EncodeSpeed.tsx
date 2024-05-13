import type { EncodeSpeedFragment$key } from '@/__generated__/EncodeSpeedFragment.graphql'

import { Chip } from '@giantnodes/react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment, useSubscription } from 'react-relay'

const FRAGMENT = graphql`
  fragment EncodeSpeedFragment on Encode {
    id
    speed {
      frames
      bitrate
      scale
    }
  }
`

const SUBSCRIPTION = graphql`
  subscription EncodeSpeedSubscription($where: EncodeFilterInput) {
    encode_speed_change(where: $where) {
      ...EncodeSpeedFragment
    }
  }
`

type EncodeSpeedProps = {
  $key: EncodeSpeedFragment$key
}

const EncodeSpeed: React.FC<EncodeSpeedProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  useSubscription({
    subscription: SUBSCRIPTION,
    variables: {
      variables: {
        where: {
          id: {
            eq: data.id,
          },
        },
      },
    },
  })

  if (data.speed == null) {
    return undefined
  }

  return (
    <>
      <Chip className="text-emerald-500 bg-emerald-500/20 border-emerald-500">{data.speed.frames} fps</Chip>

      <Chip className="text-emerald-500 bg-emerald-500/20 border-emerald-500">
        {filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s
      </Chip>

      <Chip className="text-emerald-500 bg-emerald-500/20 border-emerald-500">{data.speed.scale.toFixed(2)}x</Chip>
    </>
  )
}

export default EncodeSpeed
