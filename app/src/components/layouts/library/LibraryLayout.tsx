import React from 'react'

import Layout from '@/components/layouts/Layout'
import Navbar from '@/components/layouts/library/navbar/Navbar'
import Sidebar from '@/components/layouts/library/sidebar/Sidebar'

type LibraryLayoutProps = React.PropsWithChildren

const LibraryLayout: React.FC<LibraryLayoutProps> = ({ children }) => (
  <Layout navbar={<Navbar className="max-sm:hidden" />} sidebar={<Sidebar />}>
    {children}
  </Layout>
)

export default LibraryLayout
