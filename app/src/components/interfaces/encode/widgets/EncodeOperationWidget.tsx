import type { EncodeOperationWidgetFragment$key } from '@/__generated__/EncodeOperationWidgetFragment.graphql'

import { Chip, Table, Typography } from '@giantnodes/react'
import { IconCpu } from '@tabler/icons-react'
import dayjs from 'dayjs'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import { EncodePercent, EncodeSpeed, EncodeStatus } from '@/components/interfaces/encode'

const FRAGMENT = graphql`
  fragment EncodeOperationWidgetFragment on Encode {
    id
    updated_at
    machine {
      name
      user_name
      processor_type
    }
    ...EncodePercentFragment
    ...EncodeStatusFragment
    ...EncodeSpeedFragment
  }
`

type EncodeOperationWidgetProps = {
  $key: EncodeOperationWidgetFragment$key
}

const EncodeOperationWidget: React.FC<EncodeOperationWidgetProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return (
    <Table headingless aria-label="encode operation table" size="sm">
      <Table.Head>
        <Table.Column key="name" isRowHeader>
          name
        </Table.Column>

        <Table.Column key="value">value</Table.Column>
      </Table.Head>

      <Table.Body>
        <Table.Row>
          <Table.Cell>
            <Typography.Text>Status</Typography.Text>
          </Table.Cell>
          <Table.Cell className="text-right">
            <EncodeStatus $key={data} />
          </Table.Cell>
        </Table.Row>

        <Table.Row>
          <Table.Cell>
            <Typography.Text>Progress</Typography.Text>
          </Table.Cell>
          <Table.Cell className="text-right">
            <EncodePercent $key={data} />
          </Table.Cell>
        </Table.Row>

        <Table.Row>
          <Table.Cell>
            <Typography.Text>Machine</Typography.Text>
          </Table.Cell>
          <Table.Cell className="flex justify-end gap-1">
            {data.machine != null && (
              <>
                <Chip color="info">{data.machine?.name}</Chip>
                <Chip color="info">{data.machine?.user_name}</Chip>
                <Chip color="info">
                  {data.machine.processor_type === 'CPU' ? <IconCpu size={16} /> : data.machine.processor_type}
                </Chip>
              </>
            )}
          </Table.Cell>
        </Table.Row>

        <Table.Row>
          <Table.Cell>
            <Typography.Text>Speed</Typography.Text>
          </Table.Cell>
          <Table.Cell className="flex justify-end gap-1">
            <EncodeSpeed $key={data} />
          </Table.Cell>
        </Table.Row>

        {data.updated_at != null && (
          <Table.Row>
            <Table.Cell>
              <Typography.Text>Heartbeat</Typography.Text>
            </Table.Cell>
            <Table.Cell className="text-right">
              <Typography.Text>
                <Chip color="pink" title={dayjs(data.updated_at).format('L LTS')}>
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
