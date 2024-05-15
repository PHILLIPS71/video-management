'use client'

import { usePathname } from 'next/navigation'
import React from 'react'

import Layout from '@/components/layouts/Layout'
import Navbar from '@/components/layouts/library/navbar/Navbar'
import SettingSidebar from '@/components/layouts/library/sidebar/SettingSidebar'
import Sidebar from '@/components/layouts/library/sidebar/Sidebar'

type LibraryLayoutProps = React.PropsWithChildren

const LibraryLayout: React.FC<LibraryLayoutProps> = ({ children }) => {
  const router = usePathname()

  const route = router.split('/')[3]

  return (
    <Layout
      navbar={<Navbar className="max-sm:hidden" />}
      sidebar={
        <>
          <Sidebar />
          {route === 'settings' && <SettingSidebar />}
        </>
      }
    >
      {children}
    </Layout>
  )
}

export default LibraryLayout
