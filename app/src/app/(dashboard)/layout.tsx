import React from 'react'

import DefaultLayout from '@/components/layouts/dashboard/DashboardLayout'

type DashboardSegmentLayoutProps = React.PropsWithChildren & {
  dialog: React.ReactNode
}

const DashboardSegmentLayout: React.FC<DashboardSegmentLayoutProps> = ({ children, dialog }) => (
  <DefaultLayout>
    {children}
    {dialog}
  </DefaultLayout>
)

export const dynamic = 'force-dynamic'
export default DashboardSegmentLayout
