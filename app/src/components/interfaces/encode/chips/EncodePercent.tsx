import type { EncodePercentFragment$key } from '@/__generated__/EncodePercentFragment.graphql'

import { Chip } from '@giantnodes/react'
import React from 'react'
import { graphql, useFragment, useSubscription } from 'react-relay'

import { percent } from '@/utilities/numbers'

const FRAGMENT = graphql`
  fragment EncodePercentFragment on Encode {
    id
    percent
  }
`

const SUBSCRIPTION = graphql`
  subscription EncodePercentSubscription($where: EncodeFilterInput) {
    encode_progressed(where: $where) {
      ...EncodePercentFragment
    }
  }
`

type EncodePercentProps = {
  $key: EncodePercentFragment$key
}

const EncodePercent: React.FC<EncodePercentProps> = ({ $key }) => {
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

  if (data.percent == null) {
    return undefined
  }

  return <Chip color="brand">{percent(data.percent)}</Chip>
}

export default EncodePercent
