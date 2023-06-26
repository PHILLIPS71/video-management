'use client'

import React from 'react'

import Navbar from '@/layouts/components/Navbar'
import NavigationMobile from '@/layouts/components/Navbar.mobile'
import Sidebar from '@/layouts/components/Sidebar'
import SidebarMobile from '@/layouts/components/Sidebar.mobile'

type DefaultLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DefaultLayoutProps> = ({ children }) => (
  <div className="flex h-screen">
    <Sidebar className="max-md:hidden" />
    <SidebarMobile className="hidden sm:max-md:flex" />

    <div className="flex flex-col flex-grow overflow-x-hidden">
      <Navbar className="max-sm:hidden" />
      <NavigationMobile className="sm:hidden" />

      <main className="flex-grow py-6 md:py-8 px-6 overflow-y-auto">{children}</main>
    </div>
  </div>
)

export default DashboardLayout
