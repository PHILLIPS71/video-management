'use client'

import type { DashboardLayoutQuery } from '@/__generated__/DashboardLayoutQuery.graphql'

import { usePathname } from 'next/navigation'
import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import Navbar from '@/components/layouts/dashboard/navbar/Navbar'
import SettingSidebar from '@/components/layouts/dashboard/sidebar/SettingsSidebar'
import Sidebar from '@/components/layouts/dashboard/sidebar/Sidebar'
import Layout from '@/components/layouts/Layout'

const QUERY = graphql`
  query DashboardLayoutQuery($first: Int, $after: String, $order: [LibrarySortInput!]) {
    ...SidebarQuery @arguments(first: $first, after: $after, order: $order)
  }
`

type DashboardLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => {
  const router = usePathname()

  const query = useLazyLoadQuery<DashboardLayoutQuery>(QUERY, {
    first: 8,
    order: [{ name: 'ASC' }],
  })

  const route = router.split('/')[1]

  return (
    <Layout
      navbar={<Navbar />}
      sidebar={
        <>
          <Sidebar $key={query} className="max-md:hidden" />
          {route === 'settings' && <SettingSidebar />}
        </>
      }
    >
      {children}
    </Layout>
  )
}

export default DashboardLayout
