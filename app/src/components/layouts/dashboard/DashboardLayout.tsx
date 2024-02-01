'use client'

import type { DashboardLayoutQuery } from '@/__generated__/DashboardLayoutQuery.graphql'

import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import Navbar from '@/components/layouts/dashboard/navbar/Navbar'
import Sidebar from '@/components/layouts/dashboard/sidebar/Sidebar'
import Layout from '@/components/layouts/Layout'

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
    <Layout navbar={<Navbar />} sidebar={<Sidebar $key={query} className="max-md:hidden" />}>
      {children}
    </Layout>
  )
}

export default DashboardLayout
