import type { ExploreTableFileFragment$key } from '@/__generated__/ExploreTableFileFragment.graphql'

import { Chip, Typography } from '@giantnodes/react'
import { IconFile } from '@tabler/icons-react'
import React from 'react'
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
      nodes {
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
      <Typography.Paragraph>{data.path_info.name}</Typography.Paragraph>

      {data.video_streams?.nodes?.map((stream) => (
        <React.Fragment key={stream.index}>
          <Chip color="info" size="sm">
            {stream.codec}
          </Chip>
          <Chip color="brand" size="sm" title={`${stream.quality.width}x${stream.quality.height}`}>
            {stream.quality.resolution.abbreviation}
          </Chip>
          <Chip color="warning" size="sm">
            {stream.quality.aspect_ratio}
          </Chip>
        </React.Fragment>
      ))}
    </>
  )
}

export default ExploreTableFile
