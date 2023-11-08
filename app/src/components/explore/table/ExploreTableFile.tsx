import type { ExploreTableFileFragment$key } from '@/__generated__/ExploreTableFileFragment.graphql'

import { Chip, Table, Typography } from '@giantnodes/react'
import { IconFile } from '@tabler/icons-react'
import { filesize } from 'filesize'
import { graphql, useFragment } from 'react-relay'

type ExploreTableFileProps = {
  $key: ExploreTableFileFragment$key
}

const ExploreTableFile: React.FC<ExploreTableFileProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment ExploreTableFileFragment on FileSystemFile {
        id
        size
        path_info {
          name
          full_name
          extension
          directory_path
        }
        video_streams {
          codec
          quality {
            aspect_ratio
            width
            height
            resolution {
              abbreviation
            }
          }
        }
      }
    `,
    $key
  )

  return (
    <Table.Row key={data.id} align="right">
      <Table.Data>
        <div className="flex flex-row items-center gap-2">
          <IconFile size={20} />
          <Typography.Text>{data.path_info.name}</Typography.Text>

          {data.video_streams.map((stream) => (
            <>
              <Chip color="info" size="sm">
                {stream.codec}
              </Chip>
              <Chip color="brand" size="sm" title={`${stream.quality.width}x${stream.quality.height}`}>
                {stream.quality.resolution.abbreviation}
              </Chip>
              <Chip color="warning" size="sm">
                {stream.quality.aspect_ratio}
              </Chip>
            </>
          ))}
        </div>
      </Table.Data>
      <Table.Data>
        <Typography.Text>{filesize(data.size, { base: 2 })}</Typography.Text>
      </Table.Data>
    </Table.Row>
  )
}

export default ExploreTableFile
