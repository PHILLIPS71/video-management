/**
 * @generated SignedSource<<8227c90c017870d562e49d80bee8739d>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment, RefetchableFragment } from 'relay-runtime';
export type DriveStatus = "DEGRADED" | "OFFLINE" | "ONLINE" | "%future added value";
import { FragmentRefs } from "relay-runtime";
export type SidebarLibrarySegmentFragment$data = {
  readonly libraries: {
    readonly edges: ReadonlyArray<{
      readonly node: {
        readonly drive_status: DriveStatus;
        readonly id: string;
        readonly name: string;
      };
    }> | null;
    readonly pageInfo: {
      readonly hasNextPage: boolean;
    };
  } | null;
  readonly " $fragmentType": "SidebarLibrarySegmentFragment";
};
export type SidebarLibrarySegmentFragment$key = {
  readonly " $data"?: SidebarLibrarySegmentFragment$data;
  readonly " $fragmentSpreads": FragmentRefs<"SidebarLibrarySegmentFragment">;
};

const node: ReaderFragment = (function(){
var v0 = [
  "libraries"
];
return {
  "argumentDefinitions": [
    {
      "defaultValue": null,
      "kind": "LocalArgument",
      "name": "count"
    },
    {
      "defaultValue": null,
      "kind": "LocalArgument",
      "name": "cursor"
    }
  ],
  "kind": "Fragment",
  "metadata": {
    "connection": [
      {
        "count": "count",
        "cursor": "cursor",
        "direction": "forward",
        "path": (v0/*: any*/)
      }
    ],
    "refetch": {
      "connection": {
        "forward": {
          "count": "count",
          "cursor": "cursor"
        },
        "backward": null,
        "path": (v0/*: any*/)
      },
      "fragmentPathInResult": [],
      "operation": require('./SidebarLibrarySegmentPaginationQuery.graphql')
    }
  },
  "name": "SidebarLibrarySegmentFragment",
  "selections": [
    {
      "alias": "libraries",
      "args": null,
      "concreteType": "LibrariesConnection",
      "kind": "LinkedField",
      "name": "__SidebarLibrarySegment_libraries_connection",
      "plural": false,
      "selections": [
        {
          "alias": null,
          "args": null,
          "concreteType": "LibrariesEdge",
          "kind": "LinkedField",
          "name": "edges",
          "plural": true,
          "selections": [
            {
              "alias": null,
              "args": null,
              "concreteType": "Library",
              "kind": "LinkedField",
              "name": "node",
              "plural": false,
              "selections": [
                {
                  "alias": null,
                  "args": null,
                  "kind": "ScalarField",
                  "name": "id",
                  "storageKey": null
                },
                {
                  "alias": null,
                  "args": null,
                  "kind": "ScalarField",
                  "name": "name",
                  "storageKey": null
                },
                {
                  "alias": null,
                  "args": null,
                  "kind": "ScalarField",
                  "name": "drive_status",
                  "storageKey": null
                },
                {
                  "alias": null,
                  "args": null,
                  "kind": "ScalarField",
                  "name": "__typename",
                  "storageKey": null
                }
              ],
              "storageKey": null
            },
            {
              "alias": null,
              "args": null,
              "kind": "ScalarField",
              "name": "cursor",
              "storageKey": null
            }
          ],
          "storageKey": null
        },
        {
          "alias": null,
          "args": null,
          "concreteType": "PageInfo",
          "kind": "LinkedField",
          "name": "pageInfo",
          "plural": false,
          "selections": [
            {
              "alias": null,
              "args": null,
              "kind": "ScalarField",
              "name": "hasNextPage",
              "storageKey": null
            },
            {
              "alias": null,
              "args": null,
              "kind": "ScalarField",
              "name": "endCursor",
              "storageKey": null
            }
          ],
          "storageKey": null
        }
      ],
      "storageKey": null
    }
  ],
  "type": "Query",
  "abstractKey": null
};
})();

(node as any).hash = "2fdc9d699c738dd62b0aa79cac9f989c";

export default node;
