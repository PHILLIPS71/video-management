import { Navigation } from '@giantnodes/react'
import { IconReportAnalytics, IconScript } from '@tabler/icons-react'

import {
  EncodeDialogPanel,
  useEncodeDialogContext,
} from '@/components/interfaces/dashboard/encode-dialog/use-encode-dialog.hook'

const EncodeDialogSidebar = () => {
  const { panel, setPanel } = useEncodeDialogContext()

  return (
    <Navigation orientation="vertical" size="sm">
      <Navigation.Segment>
        <Navigation.Item isSelected={panel === EncodeDialogPanel.SCRIPT}>
          <Navigation.Trigger onPress={() => setPanel(EncodeDialogPanel.SCRIPT)}>
            <IconScript strokeWidth={1.5} />
          </Navigation.Trigger>
        </Navigation.Item>

        <Navigation.Item isSelected={panel === EncodeDialogPanel.ANALYTICS}>
          <Navigation.Trigger onPress={() => setPanel(EncodeDialogPanel.ANALYTICS)}>
            <IconReportAnalytics strokeWidth={1.5} />
          </Navigation.Trigger>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default EncodeDialogSidebar
