import type { EncodeDialogCommandFragment$key } from '@/__generated__/EncodeDialogCommandFragment.graphql'

import { graphql, useFragment } from 'react-relay'

import CodeBlock from '@/components/ui/code-block/CodeBlock'

const FRAGMENT = graphql`
  fragment EncodeDialogCommandFragment on Encode {
    command
  }
`

type EncodeDialogCommandProps = {
  $key: EncodeDialogCommandFragment$key
}

const EncodeDialogCommand: React.FC<EncodeDialogCommandProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return <CodeBlock language="powershell">{data.command}</CodeBlock>
}

export default EncodeDialogCommand
