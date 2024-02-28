import type { ExploreControlsFragment$key } from '@/__generated__/ExploreControlsFragment.graphql'
import type { FileSizeReturnObject } from 'filesize'

import { Alert, Typography } from '@giantnodes/react'
import { IconAlertCircleFilled, IconDeviceFloppy, IconFile, IconFolderFilled } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment } from 'react-relay'

import EncodeButton from '@/components/interfaces/explore/controls/EncodeButton'
import ScanButton from '@/components/interfaces/explore/controls/ScanButton'
import { useExploreContext } from '@/components/interfaces/explore/use-explore.hook'

const FRAGMENT = graphql`
  fragment ExploreControlsFragment on FileSystemDirectory {
    id
    size
    path_info {
      full_name
    }
  }
`

type ExploreControlsProps = {
  $key: ExploreControlsFragment$key
}

const ExploreControls: React.FC<ExploreControlsProps> = ({ $key }) => {
  const { errors } = useExploreContext()

  const data = useFragment(FRAGMENT, $key)

  const size = React.useMemo<FileSizeReturnObject>(
    () => filesize(data.size, { base: 2, output: 'object' }),
    [data.size]
  )

  return (
    <div className="flex flex-col gap-3">
      {errors.length > 0 && (
        <Alert color="danger">
          <IconAlertCircleFilled size={16} />
          <Alert.Body>
            <Alert.Heading>There was {errors.length} error with your submission</Alert.Heading>
            <Alert.List>
              {errors.map((error) => (
                <Alert.Item key={error}>{error}</Alert.Item>
              ))}
            </Alert.List>
          </Alert.Body>
        </Alert>
      )}

      <div className="flex gap-3 flex-col sm:flex-row">
        <div className="flex flex-grow gap-3 justify-around flex-wrap sm:justify-normal">
          <div className="flex items-center gap-1">
            <IconFolderFilled size={16} />
            <Typography.Text as="strong">0</Typography.Text>
            <Typography.Text>folders</Typography.Text>
          </div>

          <div className="flex items-center gap-1">
            <IconFile size={16} />
            <Typography.Text as="strong">0</Typography.Text>
            <Typography.Text>files</Typography.Text>
          </div>

          <div className="flex items-center gap-1">
            <IconDeviceFloppy size={16} />
            <Typography.Text as="strong">{size.value}</Typography.Text>
            <Typography.Text>{size.symbol}</Typography.Text>
          </div>
        </div>

        <EncodeButton />
        <ScanButton />
      </div>
    </div>
  )
}

export default ExploreControls
