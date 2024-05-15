import '@/styles/global.css'

import { clsx } from 'clsx'
import { Inter } from 'next/font/google'
import React from 'react'

import ApplicationProviders from '@/app/provider'

const inter = Inter({
  subsets: ['latin'],
  variable: '--font-inter',
})

type AppLayoutProps = React.PropsWithChildren & {
  dialog: React.ReactNode
}

const AppLayout: React.FC<AppLayoutProps> = ({ children, dialog }) => (
  <html lang="en">
    <head />
    <body className={clsx([inter.variable, 'bg-background'])}>
      <ApplicationProviders>
        {children}
        {dialog}
      </ApplicationProviders>
    </body>
  </html>
)

export default AppLayout
