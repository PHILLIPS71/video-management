import React from 'react'

import DashboardLayout from '@/components/layouts/dashboard/DashboardLayout'

type LibraryLayoutProps = React.PropsWithChildren

const LibraryLayout: React.FC<LibraryLayoutProps> = ({ children }) => <DashboardLayout>{children}</DashboardLayout>

export const dynamic = 'force-dynamic'
export default LibraryLayout
