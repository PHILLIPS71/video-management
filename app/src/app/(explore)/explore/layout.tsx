import React from 'react'

import DefaultLayout from '@/layouts/default/DefaultLayout'

type ExploreLayoutProps = React.PropsWithChildren

const DashboardLayout: React.FC<ExploreLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default DashboardLayout
