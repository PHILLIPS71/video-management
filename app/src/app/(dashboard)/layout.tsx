import React from 'react'

import DefaultLayout from '@/layouts/DefaultLayout'

type DashboardLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default DashboardLayout
