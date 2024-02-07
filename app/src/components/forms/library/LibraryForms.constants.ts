import * as z from 'zod'

// eslint-disable-next-line import/prefer-default-export
export const SlugTransform = z.string().transform((val) =>
  val
    ?.toLowerCase()
    .replaceAll(' ', '-')
    .replace(/[^a-z0-9-]+/g, '')
    .replace(/^-+/, '')
)
