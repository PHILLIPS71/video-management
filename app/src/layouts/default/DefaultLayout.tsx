'use client'

import type { DefaultLayoutQuery } from '@/__generated__/DefaultLayoutQuery.graphql'

import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import Navbar from '@/layouts/default/components/navbar/Navbar'
import Sidebar from '@/layouts/default/components/sidebar/Sidebar'

type DefaultLayoutProps = React.PropsWithChildren

const DefaultLayout: React.FC<DefaultLayoutProps> = ({ children }) => {
  const query = useLazyLoadQuery<DefaultLayoutQuery>(
    graphql`
      query DefaultLayoutQuery($first: Int, $after: String, $order: [LibrarySortInput!]) {
        ...SidebarQuery @arguments(first: $first, after: $after, order: $order)
      }
    `,
    {
      first: 8,
      order: [{ name: 'ASC' }],
    }
  )

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

export default DefaultLayout
