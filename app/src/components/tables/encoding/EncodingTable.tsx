import type { EncodingTable_EncodeCancelMutation } from '@/__generated__/EncodingTable_EncodeCancelMutation.graphql'
import type {
  EncodingTableFragment$data,
  EncodingTableFragment$key,
} from '@/__generated__/EncodingTableFragment.graphql'
import type { EncodingTableRefetchQuery } from '@/__generated__/EncodingTableRefetchQuery.graphql'

import { Button, Link, Table } from '@giantnodes/react'
import { IconProgressX } from '@tabler/icons-react'
import React from 'react'
import { graphql, useMutation, usePaginationFragment, useSubscription } from 'react-relay'

import EncodeDialog from '@/components/interfaces/dashboard/EncodeDialog'
import { EncodeBadges } from '@/components/ui'

const FRAGMENT = graphql`
  fragment EncodingTableFragment on Query
  @refetchable(queryName: "EncodingTableRefetchQuery")
  @argumentDefinitions(
    where: { type: "EncodeFilterInput" }
    first: { type: "Int" }
    after: { type: "String" }
    order: { type: "[EncodeSortInput!]" }
  ) {
    incomplete: encodes(where: $where, first: $first, after: $after, order: $order)
      @connection(key: "EncodingTableFragment_incomplete", filters: []) {
      edges {
        node {
          id
          file {
            path_info {
              name
            }
          }
          ...EncodeBadgesFragment
          ...EncodeDialogFragment
        }
      }
      pageInfo {
        hasNextPage
      }
    }
  }
`

const MUTATION = graphql`
  mutation EncodingTable_EncodeCancelMutation($input: Encode_cancelInput!) {
    encode_cancel(input: $input) {
      encode {
        status
      }
      errors {
        ... on DomainError {
          message
        }
        ... on ValidationError {
          message
        }
      }
    }
  }
`

const PROGRESSED_SUBSCRIPTION = graphql`
  subscription EncodingTableProgressedSubscription {
    encode_progressed {
      percent
    }
  }
`

const SPEED_SUBSCRIPTION = graphql`
  subscription EncodingTableSpeedSubscription {
    encode_speed_change {
      speed {
        frames
        bitrate
        scale
      }
    }
  }
`

type EncodeEntry = NonNullable<NonNullable<EncodingTableFragment$data['incomplete']>['edges']>[0]['node']

type EncodingTableProps = {
  $key: EncodingTableFragment$key
}

const EncodingTable: React.FC<EncodingTableProps> = ({ $key }) => {
  const { data, hasNext, loadNext } = usePaginationFragment<EncodingTableRefetchQuery, EncodingTableFragment$key>(
    FRAGMENT,
    $key
  )

  const [commit] = useMutation<EncodingTable_EncodeCancelMutation>(MUTATION)

  useSubscription({
    subscription: PROGRESSED_SUBSCRIPTION,
    variables: {},
  })

  useSubscription({
    subscription: SPEED_SUBSCRIPTION,
    variables: {},
  })

  const cancel = React.useCallback(
    (entry: EncodeEntry) => {
      commit({
        variables: {
          input: {
            encode_id: entry.id,
          },
        },
      })
    },
    [commit]
  )

  return (
    <>
      <Table headingless aria-label="encode table">
        <Table.Head>
          <Table.Column key="name" isRowHeader>
            name
          </Table.Column>
          <Table.Column key="stats">statistics</Table.Column>
        </Table.Head>

        <Table.Body items={data.incomplete?.edges ?? []}>
          {(item) => (
            <Table.Row id={item.node.id}>
              <Table.Cell>
                <EncodeDialog $key={item.node}>
                  <Button as={Link} color="transparent">
                    {item.node.file.path_info.name}
                  </Button>
                </EncodeDialog>
              </Table.Cell>
              <Table.Cell>
                <div className="flex flex-row justify-end gap-2">
                  <EncodeBadges $key={item.node} />

                  <Button color="neutral" size="xs" onPress={() => cancel(item.node)}>
                    <IconProgressX size={16} />
                  </Button>
                </div>
              </Table.Cell>
            </Table.Row>
          )}
        </Table.Body>
      </Table>

      {hasNext && (
        <div className="flex flex-row items-center justify-center p-2">
          <Button size="xs" onPress={() => loadNext(8)}>
            Show more
          </Button>
        </div>
      )}
    </>
  )
}

export default EncodingTable
