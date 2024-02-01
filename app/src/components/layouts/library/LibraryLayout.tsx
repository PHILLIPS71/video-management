import React from 'react'

import Navbar from '@/components/layouts/library/navbar/Navbar'
import Sidebar from '@/components/layouts/library/sidebar/Sidebar'

type LibraryLayoutProps = React.PropsWithChildren

const LibraryLayout: React.FC<LibraryLayoutProps> = ({ children }) => (
  <div className="flex h-screen">
    <Sidebar />

    <div className="flex flex-col flex-grow overflow-x-hidden">
      <Navbar className="max-sm:hidden" />

      <main className="flex-grow py-6 md:py-8 px-4 md:px-6 overflow-y-auto">{children}</main>
    </div>
  </div>
)

export default LibraryLayout
