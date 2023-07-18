/**
 * @generated SignedSource<<ee5e30c0cb928a9f24b663058bc72208>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { Fragment, ReaderFragment } from 'relay-runtime';
import { FragmentRefs } from "relay-runtime";
export type SidebarQuery$data = {
  readonly " $fragmentSpreads": FragmentRefs<"SidebarLibrarySegmentFragment">;
  readonly " $fragmentType": "SidebarQuery";
};
export type SidebarQuery$key = {
  readonly " $data"?: SidebarQuery$data;
  readonly " $fragmentSpreads": FragmentRefs<"SidebarQuery">;
};

const node: ReaderFragment = {
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
  "metadata": null,
  "name": "SidebarQuery",
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
};

(node as any).hash = "e72dde817d6f1cc586e5c6144cb565d5";

export default node;
