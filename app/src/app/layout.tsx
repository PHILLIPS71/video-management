import '@/styles/global.css'

import { clsx } from 'clsx'
import { Inter } from 'next/font/google'
import React from 'react'

import ApplicationProviders from '@/app/providers'

const inter = Inter({
  subsets: ['latin'],
  variable: '--font-inter',
})

type ApplicationLayoutProps = React.PropsWithChildren & {
  dialog: React.ReactNode
}

const ApplicationLayout: React.FC<ApplicationLayoutProps> = ({ children, dialog }) => (
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

export default ApplicationLayout
