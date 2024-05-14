import { LayoutNarrow } from '@/components/layouts'

type LibraryEncodeLayoutProps = React.PropsWithChildren

const LibraryEncodeLayout: React.FC<LibraryEncodeLayoutProps> = ({ children }) => (
  <LayoutNarrow>{children}</LayoutNarrow>
)

export default LibraryEncodeLayout
