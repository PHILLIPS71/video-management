'use client'

import { useLibraryContext } from './use-library.context'

const LibraryPage = () => {
  const { library } = useLibraryContext()

  return <p>Library Page: {library.name}</p>
}

export default LibraryPage
