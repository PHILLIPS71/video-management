import type { ExploreTableFileFragment$key } from '@/__generated__/ExploreTableFileFragment.graphql'

import { Table, Typography } from '@giantnodes/react'
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
        </div>
      </Table.Data>
      <Table.Data>
        <Typography.Text>{filesize(data.size, { base: 2 })}</Typography.Text>
      </Table.Data>
    </Table.Row>
  )
}

export default ExploreTableFile
