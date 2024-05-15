import { LayoutNarrow } from '@/components/layouts'

type RecipeLayoutProps = React.PropsWithChildren

const RecipeLayout: React.FC<RecipeLayoutProps> = ({ children }) => <LayoutNarrow>{children}</LayoutNarrow>

export default RecipeLayout
