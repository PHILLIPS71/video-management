import React from 'react'

import DefaultLayout from '@/components/layouts/dashboard/DashboardLayout'

type DashboardSegmentLayoutProps = React.PropsWithChildren

const DashboardSegmentLayout: React.FC<DashboardSegmentLayoutProps> = ({ children }) => (
  <DefaultLayout>{children}</DefaultLayout>
)

export const dynamic = 'force-dynamic'
export default DashboardSegmentLayout
