import type { ResolutionWidgetQuery } from '@/__generated__/ResolutionWidgetQuery.graphql'

import { Progress, Typography } from '@giantnodes/react'
import { IconPointFilled } from '@tabler/icons-react'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

const FRAGMENT = graphql`
  query ResolutionWidgetQuery($directory_id: ID!, $order: [FileResolutionDistributionSortInput!]) {
    file_resolution_distribution(directory_id: $directory_id, order: $order) {
      resolution {
        abbreviation
      }
      count
    }
  }
`

type ResolutionWidgetProps = {
  directory: string
}

const ResolutionWidget: React.FC<ResolutionWidgetProps> = ({ directory }) => {
  const colours = ['#178600', '#3178c6']

  const data = useLazyLoadQuery<ResolutionWidgetQuery>(FRAGMENT, {
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
          <li key={item.resolution?.abbreviation ?? 'unknown'} className="flex items-center gap-1">
            <IconPointFilled color={colours[index]} size={16} />
            <Typography.Text className="font-bold text-xs">
              {item.resolution?.abbreviation ?? 'unknown'}
            </Typography.Text>
            <Typography.Text className="text-xs">
              {(item.count / total).toLocaleString(undefined, { style: 'percent', maximumFractionDigits: 2 })}
            </Typography.Text>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default ResolutionWidget
