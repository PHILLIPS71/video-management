import React from 'react'

import Navbar from '@/layouts/components/navbar/Navbar'
import NavigationMobile from '@/layouts/components/navbar/Navbar.mobile'
import Sidebar from '@/layouts/components/sidebar/Sidebar'
import SidebarMobile from '@/layouts/components/sidebar/Sidebar.mobile'

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
