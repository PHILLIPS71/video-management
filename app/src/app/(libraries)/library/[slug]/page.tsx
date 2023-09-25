'use client'

import { Card, Chip, Table, Typography } from '@giantnodes/react'
import { IconGripVertical } from '@tabler/icons-react'

const LibraryPage = () => (
  <Card className="max-w-4xl">
    <Card.Header>Tasks</Card.Header>
    <Table size="sm">
      <Table.Body>
        <Table.Row className="cursor-grab">
          <Table.Data>
            <div className="flex flex-row items-center gap">
              <IconGripVertical size={16} />
              <Typography.Text>1883 - S01E01 - 1883.mp4</Typography.Text>
            </div>
          </Table.Data>

          <Table.Data align="right">
            <Chip color="success">in progress</Chip>
          </Table.Data>
        </Table.Row>

        <Table.Row className="cursor-grab">
          <Table.Data>
            <div className="flex flex-row items-center gap">
              <IconGripVertical size={16} />
              <Typography.Text>1883 - S01E02 - Behind Us, A Cliff.mp4</Typography.Text>
            </div>
          </Table.Data>

          <Table.Data align="right">
            <Chip color="success">in progress</Chip>
          </Table.Data>
        </Table.Row>

        <Table.Row className="cursor-grab">
          <Table.Data>
            <div className="flex flex-row items-center gap">
              <IconGripVertical size={16} />
              <Typography.Text>1883 - S01E03 - River.mp4</Typography.Text>
            </div>
          </Table.Data>

          <Table.Data align="right">
            <Chip color="warning">paused</Chip>
          </Table.Data>
        </Table.Row>

        <Table.Row className="cursor-grab">
          <Table.Data>
            <div className="flex flex-row items-center gap">
              <IconGripVertical size={16} />
              <Typography.Text>1883 - S01E04 - The Crossing.mp4</Typography.Text>
            </div>
          </Table.Data>

          <Table.Data align="right">
            <Chip color="info">queued</Chip>
          </Table.Data>
        </Table.Row>

        <Table.Row className="cursor-grab">
          <Table.Data>
            <div className="flex flex-row items-center gap">
              <IconGripVertical size={16} />
              <Typography.Text>1883 - S01E05 - The Fangs of Freedom.mp4</Typography.Text>
            </div>
          </Table.Data>

          <Table.Data align="right">
            <Chip color="info">queued</Chip>
          </Table.Data>
        </Table.Row>
      </Table.Body>
    </Table>
  </Card>
)

export default LibraryPage
