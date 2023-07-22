import React from 'react'

import DefaultLayout from '@/layouts/DefaultLayout'

type LibraryLayoutProps = React.PropsWithChildren

const LibraryLayout: React.FC<LibraryLayoutProps> = ({ children }) => <DefaultLayout>{children}</DefaultLayout>

export default LibraryLayout
