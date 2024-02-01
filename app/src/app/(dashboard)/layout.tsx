import React from 'react'

import DefaultLayout from '@/components/layouts/dashboard/DashboardLayout'

type DashboardLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default DashboardLayout
