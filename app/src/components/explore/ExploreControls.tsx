import type { ExploreControlsFragment$key } from '@/__generated__/ExploreControlsFragment.graphql'
import type { FileSizeReturnObject } from 'filesize'

import { Typography } from '@giantnodes/react'
import { IconDeviceFloppy, IconFile, IconFolderFilled } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

type ExploreControlsProps = React.PropsWithChildren & {
  $key: ExploreControlsFragment$key
}

const ExploreControls: React.FC<ExploreControlsProps> = ({ $key, children }) => {
  const data = useFragment(
    graphql`
      fragment ExploreControlsFragment on FileSystemDirectory {
        id
        size
        path_info {
          full_name
        }
      }
    `,
    $key
  )

  const size = React.useMemo<FileSizeReturnObject>(
    () => filesize(data.size, { base: 2, output: 'object' }),
    [data.size]
  )

  return (
    <div className="flex gap-3 flex-col sm:flex-row">
      <div className="flex flex-grow gap-3 justify-around flex-wrap sm:justify-normal">
        <div className="flex items-center gap-1">
          <IconFolderFilled size={16} />
          <Typography.Text as="strong">0</Typography.Text>
          <Typography.Text as="span">folders</Typography.Text>
        </div>

        <div className="flex items-center gap-1">
          <IconFile size={16} />
          <Typography.Text as="strong">0</Typography.Text>
          <Typography.Text as="span">files</Typography.Text>
        </div>

        <div className="flex items-center gap-1">
          <IconDeviceFloppy size={16} />
          <Typography.Text as="strong">{size.value}</Typography.Text>
          <Typography.Text as="span">{size.symbol}</Typography.Text>
        </div>
      </div>

      {children}
    </div>
  )
}

export default ExploreControls
