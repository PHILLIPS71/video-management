type LayoutProps = React.PropsWithChildren & {
  navbar?: React.ReactNode
  sidebar?: React.ReactNode
}

const Layout: React.FC<LayoutProps> = ({ navbar, sidebar, children }) => (
  <div className="flex h-screen">
    {sidebar}

    <div className="flex flex-col flex-grow overflow-x-hidden">
      {navbar}

      <main className="flex-grow py-4 px-5 overflow-y-auto">{children}</main>
    </div>
  </div>
)

export default Layout
