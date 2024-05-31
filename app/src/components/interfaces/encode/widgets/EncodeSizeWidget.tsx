import type { EncodeSizeWidgetFragment$key } from '@/__generated__/EncodeSizeWidgetFragment.graphql'
import type { DefaultTooltipContent } from 'recharts'

import { Card, Typography } from '@giantnodes/react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'
import { Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts'

const FRAGMENT = graphql`
  fragment EncodeSizeWidgetFragment on Encode {
    snapshots {
      nodes {
        size
        created_at
      }
    }
  }
`

type EncodeSizeWidgetProps = {
  $key: EncodeSizeWidgetFragment$key
}

const EncodeSizeWidget: React.FC<EncodeSizeWidgetProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  const bars = React.useMemo(() => {
    const snapshots = data.snapshots?.nodes?.toSorted((a, b) => a.created_at - b.created_at)

    const output = [
      {
        name: 'Original',
        size: snapshots?.at(0)?.size,
        fill: 'hsl(var(--twc-info) / 0.2)',
        stroke: 'hsl(var(--twc-info))',
      },
      {
        name: 'New',
        size: snapshots?.length === 1 ? 0 : snapshots?.at(snapshots.length - 1)?.size,
        fill: 'hsl(var(--twc-brand) / 0.2)',
        stroke: 'hsl(var(--twc-brand))',
      },
    ]

    return output
  }, [data])

  const TooltipContent: typeof DefaultTooltipContent<number, string> = React.useCallback((props) => {
    const value = props.payload?.at(0)?.value

    return (
      <Card>
        <Card.Header>
          <Typography.Paragraph size="sm">{props.label}</Typography.Paragraph>
        </Card.Header>

        {value !== undefined && (
          <Card.Body>
            <Typography.Paragraph size="xs" variant="subtitle">
              {filesize(value, { base: 2 })}
            </Typography.Paragraph>
          </Card.Body>
        )}
      </Card>
    )
  }, [])

  return (
    <ResponsiveContainer height="100%" width="100%">
      <BarChart data={bars} layout="vertical">
        <Tooltip content={TooltipContent} cursor={false} />

        <Bar dataKey="size" radius={[0, 8, 8, 0]} />

        <XAxis
          fontSize={14}
          stroke="hsl(var(--twc-partition))"
          tick={{ fill: 'hsl(var(--twc-content))' }}
          tickFormatter={(tick) => filesize(tick, { base: 2 })}
          type="number"
        />

        <YAxis
          dataKey="name"
          fontSize={14}
          stroke="unset"
          tick={{ fill: 'hsl(var(--twc-content))' }}
          tickLine={false}
          type="category"
        />
      </BarChart>
    </ResponsiveContainer>
  )
}

export default EncodeSizeWidget
