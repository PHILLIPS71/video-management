'use client'

import React from 'react'

import Navbar from '@/components/navbar'
import NavigationMobile from '@/components/navigation.mobile'
import Sidebar from '@/components/sidebar'
import SidebarMobile from '@/components/sidebar.mobile'

type DashboardLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => (
  <div className="flex h-screen">
    <Sidebar className="max-md:hidden" />
    <SidebarMobile className="hidden sm:max-md:flex" />

    <div className="flex flex-col">
      <Navbar className="max-sm:hidden" />
      <NavigationMobile className="sm:hidden" />

      <main className="py-6 md:py-8 px-6 overflow-y-scroll">{children}</main>
    </div>
  </div>
)

export default DashboardLayout
