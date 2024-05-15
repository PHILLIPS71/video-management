'use client'

import { DesignSystemProvider } from '@giantnodes/react'
import { useRouter } from 'next/navigation'
import { ThemeProvider } from 'next-themes'
import React from 'react'
import { RelayEnvironmentProvider } from 'react-relay'

import { environment } from '@/libraries/relay/environment'

import '@/libraries/dayjs'

type AppProviderProps = React.PropsWithChildren

const AppProvider: React.FC<AppProviderProps> = ({ children }) => {
  const router = useRouter()

  return (
    <RelayEnvironmentProvider environment={environment}>
      <DesignSystemProvider navigate={router.push}>
        <ThemeProvider attribute="class" defaultTheme="dark">
          {children}
        </ThemeProvider>
      </DesignSystemProvider>
    </RelayEnvironmentProvider>
  )
}

export default AppProvider
