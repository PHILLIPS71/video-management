import type { ExploreTableFileFragment$key } from '@/__generated__/ExploreTableFileFragment.graphql'

import { Chip, Typography } from '@giantnodes/react'
import { IconFile } from '@tabler/icons-react'
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
        transcodes {
          id
        }
      }
    `,
    $key
  )

  return (
    <>
      <IconFile size={20} />
      <Typography.Text>{data.path_info.name}</Typography.Text>

      {data.video_streams?.map((stream) => (
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

      {data.transcodes && data.transcodes.length > 0 && (
        <Chip color="info" size="sm">
          transcoding
        </Chip>
      )}
    </>
  )
}

export default ExploreTableFile
