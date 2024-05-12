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
      <Chip color="info">{data.speed.frames} fps</Chip>

      <Chip color="info">{filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s</Chip>

      <Chip color="info">{data.speed.scale.toFixed(2)}x</Chip>
    </>
  )
}

export default EncodeSpeed
