'use client'

import { Inter } from 'next/font/google'
import React from 'react'
import { RelayEnvironmentProvider } from 'react-relay'

import { environment } from '@/libs/relay/environment'

import '@/app/global.css'

const inter = Inter({
  subsets: ['latin'],
  weight: ['400', '500'],
})

type ApplicationLayoutProps = React.PropsWithChildren

const ApplicationLayout: React.FC<ApplicationLayoutProps> = ({ children }) => (
  <html lang="en">
    <body className={inter.className}>
      <RelayEnvironmentProvider environment={environment}>{children}</RelayEnvironmentProvider>
    </body>
  </html>
)

export default ApplicationLayout
