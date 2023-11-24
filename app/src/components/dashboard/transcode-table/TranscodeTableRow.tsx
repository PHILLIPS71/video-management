import type { TranscodeTableRow_TranscodeCancelMutation } from '@/__generated__/TranscodeTableRow_TranscodeCancelMutation.graphql'
import type { TranscodeStatus, TranscodeTableRowFragment$key } from '@/__generated__/TranscodeTableRowFragment.graphql'

import { Chip, Table, Typography } from '@giantnodes/react'
import { IconProgressX } from '@tabler/icons-react'
import { filesize } from 'filesize'
import { graphql, useFragment, useMutation } from 'react-relay'

type TranscodeTablePropsProps = {
  $key: TranscodeTableRowFragment$key
}

const TranscodeTableRow: React.FC<TranscodeTablePropsProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment TranscodeTableRowFragment on Transcode {
        id
        status
        percent
        speed {
          frames
          bitrate
          scale
        }
        file {
          id
          library {
            name
          }
          path_info {
            name
          }
        }
      }
    `,
    $key
  )

  const [commit] = useMutation<TranscodeTableRow_TranscodeCancelMutation>(graphql`
    mutation TranscodeTableRow_TranscodeCancelMutation($input: File_transcode_cancelInput!) {
      file_transcode_cancel(input: $input) {
        transcode {
          status
        }
        errors {
          ... on DomainError {
            message
          }
          ... on ValidationError {
            message
          }
        }
      }
    }
  `)

  const percent = (value: number): string =>
    Intl.NumberFormat('en-US', {
      style: 'percent',
      maximumFractionDigits: 2,
    }).format(value)

  const getStatusColour = (status: TranscodeStatus) => {
    switch (status) {
      case 'SUBMITTED':
        return 'info'

      case 'QUEUED':
        return 'info'

      case 'TRANSCODING':
        return 'success'

      case 'CANCELLING':
      case 'DEGRADED':
        return 'warning'

      case 'FAILED':
        return 'danger'

      case 'CANCELLED':
        return 'neutral'

      case 'COMPLETED':
        return 'success'

      default:
        return 'danger'
    }
  }

  const cancel = () => {
    commit({
      variables: {
        input: {
          file_id: data.file.id,
          transcode_id: data.id,
        },
      },
    })
  }

  return (
    <Table.Row>
      <Table.Data>
        <div className="flex flex-row items-center gap">
          <Typography.Text>{data.file.path_info.name}</Typography.Text>
        </div>
      </Table.Data>

      <Table.Data align="right">
        <div className="flex flex-row items-center justify-end gap-2">
          {data.status !== 'COMPLETED' && (
            <>
              {data.speed != null && (
                <>
                  <Chip color="info">{data.speed.frames} fps</Chip>
                  <Chip color="info">{filesize(data.speed.bitrate * 0.125, { bits: true }).toLowerCase()}/s</Chip>
                  <Chip color="info">{data.speed.scale.toFixed(2)}x</Chip>
                </>
              )}

              {data.percent != null && <Chip color="info">{percent(data.percent)}</Chip>}
            </>
          )}

          <Chip color={getStatusColour(data.status)}>{data.status.toLowerCase()}</Chip>

          {data.status !== 'COMPLETED' && data.status !== 'CANCELLING' && data.status !== 'CANCELLED' && (
            <div className="cursor-pointer text-shark-200" onClick={() => cancel()}>
              <IconProgressX size={16} />
            </div>
          )}
        </div>
      </Table.Data>
    </Table.Row>
  )
}

export default TranscodeTableRow
