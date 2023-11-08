import type { ExploreControls_DirectoryProbeMutation } from '@/__generated__/ExploreControls_DirectoryProbeMutation.graphql'
import type { ExploreControlsFragment$key } from '@/__generated__/ExploreControlsFragment.graphql'
import type { FileSizeReturnObject } from 'filesize'

import { Button, Typography } from '@giantnodes/react'
import { IconDeviceFloppy, IconFile, IconFolderFilled, IconFolderSearch } from '@tabler/icons-react'
import { filesize } from 'filesize'
import React from 'react'
import { graphql, useFragment, useMutation } from 'react-relay'

type ExploreControlsProps = {
  $key: ExploreControlsFragment$key
}

const ExploreControls: React.FC<ExploreControlsProps> = ({ $key }) => {
  const data = useFragment(
    graphql`
      fragment ExploreControlsFragment on FileSystemDirectory {
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

  const [commit, isLoading] = useMutation<ExploreControls_DirectoryProbeMutation>(graphql`
    mutation ExploreControls_LibraryProbeMutation($input: Directory_probeInput!) {
      directory_probe(input: $input) {
        string
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

  const onScanClick = () => {
    commit({
      variables: {
        input: {
          path: data.path_info.full_name,
        },
      },
    })
  }

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

      <Button color="brand" disabled={isLoading} size="sm" onClick={() => onScanClick()}>
        <IconFolderSearch size={16} /> Scan
      </Button>
    </div>
  )
}

export default ExploreControls
