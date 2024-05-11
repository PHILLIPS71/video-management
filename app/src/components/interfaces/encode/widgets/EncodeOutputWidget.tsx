import type { EncodeOutputWidgetFragment$key } from '@/__generated__/EncodeOutputWidgetFragment.graphql'
import type { EncodeOutputWidgetSubscription } from '@/__generated__/EncodeOutputWidgetSubscription.graphql'

import { graphql, useFragment, useSubscription } from 'react-relay'

import CodeBlock from '@/components/ui/code-block/CodeBlock'
import ScrollAnchor from '@/components/ui/ScrollAnchor'

const FRAGMENT = graphql`
  fragment EncodeOutputWidgetFragment on Encode {
    id
    output
  }
`

const SUBSCRIPTION = graphql`
  subscription EncodeOutputWidgetSubscription($where: EncodeFilterInput) {
    encode_outputted(where: $where) {
      ...EncodeOutputWidgetFragment
    }
  }
`

type EncodeOutputWidgetProps = {
  $key: EncodeOutputWidgetFragment$key
}

const EncodeOutputWidget: React.FC<EncodeOutputWidgetProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  useSubscription<EncodeOutputWidgetSubscription>({
    subscription: SUBSCRIPTION,
    variables: {
      where: {
        id: {
          eq: data.id,
        },
      },
    },
  })

  return (
    <ScrollAnchor>
      <CodeBlock language="powershell">{data.output}</CodeBlock>
    </ScrollAnchor>
  )
}

export default EncodeOutputWidget
