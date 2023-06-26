import React from 'react'

import DefaultLayout from '@/layouts/DefaultLayout'

type ExploreLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<ExploreLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default DashboardLayout
