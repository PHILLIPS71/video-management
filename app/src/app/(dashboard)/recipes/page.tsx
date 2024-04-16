'use client'

import type { page_RecipesPageQuery } from '@/__generated__/page_RecipesPageQuery.graphql'

import { Button, Card, Typography } from '@giantnodes/react'
import React, { Suspense } from 'react'
import { graphql, useLazyLoadQuery } from 'react-relay'

import { RecipeDialog, RecipeTable } from '@/components/interfaces/recipes'

const QUERY = graphql`
  query page_RecipesPageQuery($first: Int, $after: String, $order: [RecipeSortInput!]) {
    ...RecipeTableFragment @arguments(first: $first, after: $after, order: $order)
  }
`

const RecipeListPage: React.FC = () => {
  const query = useLazyLoadQuery<page_RecipesPageQuery>(QUERY, {
    first: 25,
    order: [{ name: 'ASC' }],
  })

  return (
    <div className="mx-auto max-w-6xl">
      <div className="flex flex-col gap-6">
        <div className="flex lg:flex-row flex-col gap-2">
          <Typography.HeadingLevel>
            <div className="flex-grow">
              <Typography.Heading as={3}>Recipes</Typography.Heading>
              <Typography.Paragraph variant="subtitle">
                Recipes are a set of predefined configurations that will be sent to ffmpeg during an encoding operation.
              </Typography.Paragraph>
            </div>

            <div className="mt-auto ml-auto">
              <RecipeDialog>
                <Button size="xs">Create new recipe</Button>
              </RecipeDialog>
            </div>
          </Typography.HeadingLevel>
        </div>

        <Card>
          <Suspense>
            <RecipeTable $key={query} />
          </Suspense>
        </Card>
      </div>
    </div>
  )
}

export default RecipeListPage
