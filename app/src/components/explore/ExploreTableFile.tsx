import type { ExploreTableFileFragment$key } from '@/__generated__/ExploreTableFileFragment.graphql'

import { Chip, Typography } from '@giantnodes/react'
import { IconFile } from '@tabler/icons-react'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
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
      index
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
    encodes {
      id
    }
  }
`

type ExploreTableFileProps = {
  $key: ExploreTableFileFragment$key
}

const ExploreTableFile: React.FC<ExploreTableFileProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

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

      {data.encodes && data.encodes.length > 0 && (
        <Chip color="info" size="sm">
          encoding
        </Chip>
      )}
    </>
  )
}

export default ExploreTableFile
