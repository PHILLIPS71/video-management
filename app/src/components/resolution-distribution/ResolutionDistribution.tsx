import type { ResolutionDistributionQuery } from '@/__generated__/ResolutionDistributionQuery.graphql'

import { Progress, Typography } from '@giantnodes/react'
import { IconPointFilled } from '@tabler/icons-react'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

const FRAGMENT = graphql`
  query ResolutionDistributionQuery($directory_id: ID!, $order: [FileResolutionDistributionSortInput!]) {
    file_resolution_distribution(directory_id: $directory_id, order: $order) {
      resolution {
        abbreviation
      }
      count
    }
  }
`

export type ResolutionDistributionProps = {
  directory: string
}

export const ResolutionDistribution: React.FC<ResolutionDistributionProps> = ({ directory }) => {
  const colours = ['#178600', '#3178c6']

  const data = useLazyLoadQuery<ResolutionDistributionQuery>(FRAGMENT, {
    directory_id: directory,
    order: [{ count: 'DESC' }],
  })

  const total = React.useMemo<number>(
    () =>
      data.file_resolution_distribution.reduce<number>((accu, item) => {
        accu += item.count

        return accu
      }, 0),
    [data.file_resolution_distribution]
  )

  return (
    <div className="flex flex-col gap-2">
      <Progress>
        {data.file_resolution_distribution.map((item, index) => (
          <Progress.Bar
            key={item.resolution?.abbreviation ?? 'unknown'}
            color={colours[index]}
            width={(item.count / total) * 100}
          />
        ))}
      </Progress>

      <ul className="flex flex-wrap gap-4">
        {data.file_resolution_distribution.map((item, index) => (
          <li key={item.resolution?.abbreviation} className="flex items-center gap-1">
            <IconPointFilled color={colours[index]} size={16} />
            <Typography.Text as="span" className="font-bold text-xs">
              {item.resolution?.abbreviation ?? 'unknown'}
            </Typography.Text>
            <Typography.Text as="span" className="text-xs">
              {(item.count / total).toLocaleString(undefined, { style: 'percent', maximumFractionDigits: 2 })}
            </Typography.Text>
          </li>
        ))}
      </ul>
    </div>
  )
}
