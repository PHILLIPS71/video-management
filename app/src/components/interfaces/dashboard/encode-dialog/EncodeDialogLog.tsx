import type { EncodeDialogLogFragment$key } from '@/__generated__/EncodeDialogLogFragment.graphql'
import type { EncodeDialogLogSubscription } from '@/__generated__/EncodeDialogLogSubscription.graphql'

import { graphql, useFragment, useSubscription } from 'react-relay'

import CodeBlock from '@/components/ui/code-block/CodeBlock'
import ScrollAnchor from '@/components/ui/ScrollAnchor'

const FRAGMENT = graphql`
  fragment EncodeDialogLogFragment on Encode {
    id
    output
  }
`

const SUBSCRIPTION = graphql`
  subscription EncodeDialogLogSubscription($where: EncodeFilterInput) {
    encode_outputted(where: $where) {
      ...EncodeDialogLogFragment
    }
  }
`

type EncodeDialogLogProps = {
  $key: EncodeDialogLogFragment$key
}

const EncodeDialogLog: React.FC<EncodeDialogLogProps> = ({ $key }) => {
  const data = useFragment(FRAGMENT, $key)

  useSubscription<EncodeDialogLogSubscription>({
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

export default EncodeDialogLog
