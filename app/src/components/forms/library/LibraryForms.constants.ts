import * as z from 'zod'

export const SlugTransform = z.string().transform((val) =>
  val
    ?.toLowerCase()
    .replaceAll(' ', '-')
    .replace(/[^a-z0-9-]+/g, '')
    .replace(/^-+/, '')
)
