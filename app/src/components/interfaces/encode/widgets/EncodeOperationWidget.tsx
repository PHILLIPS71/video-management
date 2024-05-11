import type { EncodeOperationWidgetFragment$key } from '@/__generated__/EncodeOperationWidgetFragment.graphql'

import { Chip, Table, Typography } from '@giantnodes/react'
import dayjs from 'dayjs'
import { filesize } from 'filesize'
import { graphql, useFragment } from 'react-relay'

import { percent } from '@/utilities/numbers'

const FRAGMENT = graphql`
  fragment EncodeOperationWidgetFragment on Encode {
    percent
    updated_at
    machine {
      name
      user_name
    }
    speed {
      bitrate
      frames
      scale
    }
  }
`

type EncodeOperationWidgetProps = {
  $key: EncodeOperationWidgetFragment$key
}

const EncodeOperationWidget: React.FC<EncodeOperationWidgetProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return (
    <Table headingless size="sm">
      <Table.Head>
        <Table.Column key="name" isRowHeader>
          name
        </Table.Column>

        <Table.Column key="name">value</Table.Column>
      </Table.Head>

      <Table.Body>
        {data.percent != null && (
          <Table.Row>
            <Table.Cell>
              <Typography.Text>Progress</Typography.Text>
            </Table.Cell>
            <Table.Cell className="text-right">
              <Chip color="brand">{percent(data.percent)}</Chip>
            </Table.Cell>
          </Table.Row>
        )}

        <Table.Row>
          <Table.Cell>
            <Typography.Text>Machine</Typography.Text>
          </Table.Cell>
          <Table.Cell className="flex justify-end gap-1">
            <Chip color="info">{data.machine?.name}</Chip>
            <Chip color="info">{data.machine?.user_name}</Chip>
          </Table.Cell>
        </Table.Row>

        {data.speed != null && (
          <Table.Row>
            <Table.Cell>
              <Typography.Text>Speed</Typography.Text>
            </Table.Cell>
            <Table.Cell className="flex justify-end gap-1">
              <Chip color="warning">{data.speed.frames} fps</Chip>

              <Chip color="warning">{filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s</Chip>

              <Chip color="warning">{data.speed.scale.toFixed(2)}x</Chip>
            </Table.Cell>
          </Table.Row>
        )}

        {data.updated_at != null && (
          <Table.Row>
            <Table.Cell>
              <Typography.Text>Heartbeat</Typography.Text>
            </Table.Cell>
            <Table.Cell className="text-right">
              <Typography.Text>
                <Chip color="danger" title={dayjs(data.updated_at).format('L LT')}>
                  {dayjs(data.updated_at).fromNow()}
                </Chip>
              </Typography.Text>
            </Table.Cell>
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  )
}

export default EncodeOperationWidget
