'use client'

import type { DashboardLayoutQuery } from '@/__generated__/DashboardLayoutQuery.graphql'

import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import Navbar from '@/components/layouts/dashboard/navbar/Navbar'
import Sidebar from '@/components/layouts/dashboard/sidebar/Sidebar'

const QUERY = graphql`
  query DashboardLayoutQuery($first: Int, $after: String, $order: [LibrarySortInput!]) {
    ...SidebarQuery @arguments(first: $first, after: $after, order: $order)
  }
`

type DashboardLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => {
  const query = useLazyLoadQuery<DashboardLayoutQuery>(QUERY, {
    first: 8,
    order: [{ name: 'ASC' }],
  })

  return (
    <div className="flex h-screen">
      <Sidebar $key={query} className="max-md:hidden" />

      <div className="flex flex-col flex-grow overflow-x-hidden">
        <Navbar />

        <main className="flex-grow py-6 md:py-8 px-4 md:px-6 overflow-y-auto">{children}</main>
      </div>
    </div>
  )
}

export default DashboardLayout
