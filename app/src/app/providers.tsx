'use client'

import { ThemeProvider } from 'next-themes'
import { RelayEnvironmentProvider } from 'react-relay'

import { environment } from '@/libs/relay/environment'

type ApplicationProvidersProps = React.PropsWithChildren

const ApplicationProviders: React.FC<ApplicationProvidersProps> = ({ children }) => (
  <RelayEnvironmentProvider environment={environment}>
    <ThemeProvider attribute="class" defaultTheme="dark">
      {children}
    </ThemeProvider>
  </RelayEnvironmentProvider>
)

export default ApplicationProviders
