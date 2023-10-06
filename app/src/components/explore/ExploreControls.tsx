import type { ExploreControlsFragment$key } from '@/__generated__/ExploreControlsFragment.graphql'

import { Button, Typography } from '@giantnodes/react'
import { IconDeviceFloppy, IconFile, IconFolderFilled, IconFolderSearch } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

type ExploreControlsProps = {
  $key: ExploreControlsFragment$key
}

type SizeObject = {
  value: number
  symbol: string
  exponent: number
  unit: string
}

const ExploreControls: React.FC<ExploreControlsProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment ExploreControlsFragment on FileSystemDirectory {
        size
      }
    `,
    $key
  )

  const size = React.useMemo<SizeObject>(
    () => filesize(data.size, { base: 2, output: 'object' }) as SizeObject,
    [data.size]
  )

  return (
    <div className="flex gap-4 flex-col sm:flex-row">
      <div className="flex flex-grow gap-4 justify-around flex-wrap sm:justify-normal">
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

      <Button color="brand" size="sm">
        <IconFolderSearch size={16} /> Scan
      </Button>
    </div>
  )
}

export default ExploreControls
