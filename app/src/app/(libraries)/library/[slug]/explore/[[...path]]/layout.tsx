import { LayoutNarrow } from '@/components/layouts'

type LibraryExploreLayoutProps = React.PropsWithChildren

const LibraryExploreLayout: React.FC<LibraryExploreLayoutProps> = ({ children }) => (
  <LayoutNarrow>{children}</LayoutNarrow>
)

export default LibraryExploreLayout
