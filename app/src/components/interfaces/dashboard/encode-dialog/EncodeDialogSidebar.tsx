import { Navigation } from '@giantnodes/react'
import { IconReportAnalytics, IconScript } from '@tabler/icons-react'

import {
  EncodeDialogPage,
  useEncodeDialogContext,
} from '@/components/interfaces/dashboard/encode-dialog/use-encode-dialog.hook'

const EncodeDialogSidebar = () => {
  const { page, setPage } = useEncodeDialogContext()

  return (
    <Navigation orientation="vertical" size="sm">
      <Navigation.Segment>
        <Navigation.Item onClick={() => setPage(EncodeDialogPage.SCRIPT)}>
          <Navigation.Link href="/" isSelected={page === EncodeDialogPage.SCRIPT}>
            <IconScript href="/" strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>

        <Navigation.Item onClick={() => setPage(EncodeDialogPage.ANALYTICS)}>
          <Navigation.Link href="/" isSelected={page === EncodeDialogPage.ANALYTICS}>
            <IconReportAnalytics href="/" strokeWidth={1.5} />
          </Navigation.Link>
        </Navigation.Item>
      </Navigation.Segment>
    </Navigation>
  )
}

export default EncodeDialogSidebar
