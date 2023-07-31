import React from 'react'

import LibraryLayout from '@/layouts/library/LibraryLayout'

type LibraryPageLayoutProps = React.PropsWithChildren

const LibraryPageLayout: React.FC<LibraryPageLayoutProps> = ({ children }) => <LibraryLayout>{children}</LibraryLayout>

export default LibraryPageLayout
