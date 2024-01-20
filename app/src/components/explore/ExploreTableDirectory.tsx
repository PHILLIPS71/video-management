import type { ExploreTableDirectoryFragment$key } from '@/__generated__/ExploreTableDirectoryFragment.graphql'

import { Link } from '@giantnodes/react'
import { IconFolderFilled } from '@tabler/icons-react'
import NextLink from 'next/link'
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

      <NextLink legacyBehavior href={`${pathname}/${data.path_info.name}`}>
        <Link>{data.path_info.name}</Link>
      </NextLink>
    </>
  )
}

export default ExploreTableDirectory
