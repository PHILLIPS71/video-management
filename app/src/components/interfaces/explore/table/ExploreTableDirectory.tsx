import type { ExploreTableDirectoryFragment$key } from '@/__generated__/ExploreTableDirectoryFragment.graphql'

import { Link } from '@giantnodes/react'
import { IconFolderFilled } from '@tabler/icons-react'
import { usePathname } from 'next/navigation'
import { graphql, useFragment } from 'react-relay'

const FRAGMENT = graphql`
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
`

type ExploreTableDirectoryProps = {
  $key: ExploreTableDirectoryFragment$key
}

const ExploreTableDirectory: React.FC<ExploreTableDirectoryProps> = ({ $key }) => {
  const pathname = usePathname()

  const data = useFragment(FRAGMENT, $key)

  return (
    <>
      <IconFolderFilled size={20} />

      <Link href={`${pathname}/${data.path_info.name}`}>{data.path_info.name}</Link>
    </>
  )
}

export default ExploreTableDirectory
