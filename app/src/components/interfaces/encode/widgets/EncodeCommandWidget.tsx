import type { EncodeCommandWidgetFragment$key } from '@/__generated__/EncodeCommandWidgetFragment.graphql'

import { graphql, useFragment } from 'react-relay'

import { CodeBlock } from '@/components/ui'

const FRAGMENT = graphql`
  fragment EncodeCommandWidgetFragment on Encode {
    command
  }
`

type EncodeCommandWidgetProps = {
  $key: EncodeCommandWidgetFragment$key
}

const EncodeCommandWidget: React.FC<EncodeCommandWidgetProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  return <CodeBlock language="powershell">{data.command}</CodeBlock>
}

export default EncodeCommandWidget
