/**
 * @generated SignedSource<<ea2d177e59e5889800e4fcca6285f47f>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest, Query } from 'relay-runtime';
import { FragmentRefs } from "relay-runtime";
export type SidebarLibrarySegmentPaginationQuery$variables = {
  count?: number | null;
  cursor?: string | null;
};
export type SidebarLibrarySegmentPaginationQuery$data = {
  readonly " $fragmentSpreads": FragmentRefs<"SidebarLibrarySegmentFragment">;
};
export type SidebarLibrarySegmentPaginationQuery = {
  response: SidebarLibrarySegmentPaginationQuery$data;
  variables: SidebarLibrarySegmentPaginationQuery$variables;
};

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "defaultValue": 5,
    "kind": "LocalArgument",
    "name": "count"
  },
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "cursor"
  }
],
v1 = [
  {
    "kind": "Variable",
    "name": "after",
    "variableName": "cursor"
  },
  {
    "kind": "Variable",
    "name": "first",
    "variableName": "count"
  }
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "SidebarLibrarySegmentPaginationQuery",
    "selections": [
      {
        "args": [
          {
            "kind": "Variable",
            "name": "count",
            "variableName": "count"
          },
          {
            "kind": "Variable",
            "name": "cursor",
            "variableName": "cursor"
          }
        ],
        "kind": "FragmentSpread",
        "name": "SidebarLibrarySegmentFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "SidebarLibrarySegmentPaginationQuery",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "LibrariesConnection",
        "kind": "LinkedField",
        "name": "libraries",
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
      },
      {
        "alias": null,
        "args": (v1/*: any*/),
        "filters": null,
        "handle": "connection",
        "key": "SidebarLibrarySegment_libraries",
        "kind": "LinkedHandle",
        "name": "libraries"
      }
    ]
  },
  "params": {
    "cacheID": "2cfb1ab22ce7e476fa8ee409a1a7f226",
    "id": null,
    "metadata": {},
    "name": "SidebarLibrarySegmentPaginationQuery",
    "operationKind": "query",
    "text": "query SidebarLibrarySegmentPaginationQuery(\n  $count: Int = 5\n  $cursor: String\n) {\n  ...SidebarLibrarySegmentFragment_1G22uz\n}\n\nfragment SidebarLibrarySegmentFragment_1G22uz on Query {\n  libraries(after: $cursor, first: $count) {\n    edges {\n      node {\n        id\n        name\n        drive_status\n        __typename\n      }\n      cursor\n    }\n    pageInfo {\n      hasNextPage\n      endCursor\n    }\n  }\n}\n"
  }
};
})();

(node as any).hash = "7681b00be146e1c0a11779e1cdbfe3d9";

export default node;
