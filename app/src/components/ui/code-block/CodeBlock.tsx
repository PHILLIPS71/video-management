import type { SyntaxHighlighterProps } from 'react-syntax-highlighter'

import SyntaxHighlighter from 'react-syntax-highlighter'
import { atomOneDark } from 'react-syntax-highlighter/dist/esm/styles/hljs'

type CodeBlockProps = Pick<SyntaxHighlighterProps, 'language'> & {
  children: string | string[]
}

const CodeBlock: React.FC<CodeBlockProps> = ({ children, ...rest }) => (
  <SyntaxHighlighter
    wrapLines
    wrapLongLines
    className="!bg-foreground rounded-md text-xs"
    style={atomOneDark}
    {...rest}
  >
    {children}
  </SyntaxHighlighter>
)

export default CodeBlock
