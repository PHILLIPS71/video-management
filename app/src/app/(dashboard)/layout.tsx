import type { Metadata } from 'next'

import React from 'react'

import DefaultLayout from '@/components/layouts/dashboard/DashboardLayout'

type DashboardLayoutProps = React.PropsWithChildren

export const metadata: Metadata = {
  title: 'Dashboard',
}

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default DashboardLayout
