type LayoutNarrowProps = React.PropsWithChildren

const LayoutNarrow: React.FC<LayoutNarrowProps> = ({ children }) => <div className="max-w-6xl mx-auto">{children}</div>

export default LayoutNarrow
