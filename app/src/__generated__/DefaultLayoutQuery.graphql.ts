/**
 * @generated SignedSource<<8b0d1c816cc04c97596d78831b90f330>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest, Query } from 'relay-runtime';
import { FragmentRefs } from "relay-runtime";
export type DefaultLayoutQuery$variables = {
  count?: number | null;
  cursor?: string | null;
};
export type DefaultLayoutQuery$data = {
  readonly " $fragmentSpreads": FragmentRefs<"SidebarQuery">;
};
export type DefaultLayoutQuery = {
  response: DefaultLayoutQuery$data;
  variables: DefaultLayoutQuery$variables;
};

const node: ConcreteRequest = (function(){
var v0 = {
  "defaultValue": null,
  "kind": "LocalArgument",
  "name": "count"
},
v1 = {
  "defaultValue": null,
  "kind": "LocalArgument",
  "name": "cursor"
},
v2 = [
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
    "argumentDefinitions": [
      (v0/*: any*/),
      (v1/*: any*/)
    ],
    "kind": "Fragment",
    "metadata": null,
    "name": "DefaultLayoutQuery",
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
        "name": "SidebarQuery"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [
      (v1/*: any*/),
      (v0/*: any*/)
    ],
    "kind": "Operation",
    "name": "DefaultLayoutQuery",
    "selections": [
      {
        "alias": null,
        "args": (v2/*: any*/),
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
        "args": (v2/*: any*/),
        "filters": null,
        "handle": "connection",
        "key": "SidebarLibrarySegment_libraries",
        "kind": "LinkedHandle",
        "name": "libraries"
      }
    ]
  },
  "params": {
    "cacheID": "b39e4b9268ea25bfc73d8a68b26b7cf0",
    "id": null,
    "metadata": {},
    "name": "DefaultLayoutQuery",
    "operationKind": "query",
    "text": "query DefaultLayoutQuery(\n  $cursor: String\n  $count: Int\n) {\n  ...SidebarQuery_1G22uz\n}\n\nfragment SidebarLibrarySegmentFragment_1G22uz on Query {\n  libraries(after: $cursor, first: $count) {\n    edges {\n      node {\n        id\n        name\n        drive_status\n        __typename\n      }\n      cursor\n    }\n    pageInfo {\n      hasNextPage\n      endCursor\n    }\n  }\n}\n\nfragment SidebarQuery_1G22uz on Query {\n  ...SidebarLibrarySegmentFragment_1G22uz\n}\n"
  }
};
})();

(node as any).hash = "9b83b17532f986b841f0922da2d63445";

export default node;
