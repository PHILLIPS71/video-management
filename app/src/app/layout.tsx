import '@/styles/global.css'

import { clsx } from 'clsx'
import { Inter } from 'next/font/google'
import React from 'react'

import ApplicationProviders from '@/app/providers'

const inter = Inter({
  subsets: ['latin'],
  variable: '--font-inter',
})

type ApplicationLayoutProps = React.PropsWithChildren

const ApplicationLayout: React.FC<ApplicationLayoutProps> = ({ children }) => (
  <html lang="en">
    <head />
    <body className={clsx([inter.variable, 'dark:bg-bunker-800'])}>
      <ApplicationProviders>{children}</ApplicationProviders>
    </body>
  </html>
)

export default ApplicationLayout
