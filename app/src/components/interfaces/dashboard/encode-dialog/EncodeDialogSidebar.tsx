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
        <Navigation.Item onClick={() => setPanel(EncodeDialogPanel.SCRIPT)}>
          <Navigation.Link href="/" isSelected={panel === EncodeDialogPanel.SCRIPT}>
            <IconScript strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item onClick={() => setPanel(EncodeDialogPanel.ANALYTICS)}>
          <Navigation.Link href="/" isSelected={panel === EncodeDialogPanel.ANALYTICS}>
            <IconReportAnalytics strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default EncodeDialogSidebar
