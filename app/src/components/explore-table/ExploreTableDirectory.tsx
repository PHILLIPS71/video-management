import type { ExploreTableDirectoryFragment$key } from '@/__generated__/ExploreTableDirectoryFragment.graphql'

import { Link, Table, Typography } from '@giantnodes/react'
import { IconFolderFilled } from '@tabler/icons-react'
import { filesize } from 'filesize'
import NextLink from 'next/link'
import { usePathname } from 'next/navigation'
import { graphql, useFragment } from 'react-relay'

type ExploreTableDirectoryProps = {
  $key: ExploreTableDirectoryFragment$key
}

const ExploreTableDirectory: React.FC<ExploreTableDirectoryProps> = ({ $key }) => {
  const pathname = usePathname()

  const data = useFragment(
    graphql`
      fragment ExploreTableDirectoryFragment on FileSystemDirectory {
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
          <IconFolderFilled size={20} />
          <NextLink legacyBehavior href={`${pathname}/${data.path_info.name}`}>
            <Link>{data.path_info.name}</Link>
          </NextLink>
        </div>
      </Table.Data>
      <Table.Data>
        <Typography.Text>{filesize(data.size, { base: 2 })}</Typography.Text>
      </Table.Data>
    </Table.Row>
  )
}

export default ExploreTableDirectory
