'use client'

import type { DefaultLayoutQuery } from '@/__generated__/DefaultLayoutQuery.graphql'

import React from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import Navbar from '@/layouts/components/navbar/Navbar'
import NavigationMobile from '@/layouts/components/navbar/Navbar.mobile'
import Sidebar from '@/layouts/components/sidebar/Sidebar'

type DefaultLayoutProps = React.PropsWithChildren

const DefaultLayout: React.FC<DefaultLayoutProps> = ({ children }) => {
  const query = useLazyLoadQuery<DefaultLayoutQuery>(
    graphql`
      query DefaultLayoutQuery($cursor: String, $count: Int) {
        ...SidebarQuery @arguments(cursor: $cursor, count: $count)
      }
    `,
    {
      count: 8,
    }
  )

  return (
    <div className="flex h-screen">
      <Sidebar $key={query} className="max-md:hidden" />

      <div className="flex flex-col flex-grow overflow-x-hidden">
        <Navbar className="max-sm:hidden" />
        <NavigationMobile className="sm:hidden" />

        <main className="flex-grow py-6 md:py-8 px-6 overflow-y-auto">{children}</main>
      </div>
    </div>
  )
}

export default DefaultLayout
