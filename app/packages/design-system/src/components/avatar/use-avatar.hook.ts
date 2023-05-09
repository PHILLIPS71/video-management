import type { AvatarVariantProps } from '@/components/avatar/Avatar.styles'

import React from 'react'

import avatar from '@/components/avatar/Avatar.styles'

export type UseAvatarProps = AvatarVariantProps

export const useAvatar = (props: UseAvatarProps) => {
  const { color, radius, size, bordered, zoomed } = props

  const slots = React.useMemo(
    () =>
      avatar({
        color,
        radius,
        size,
        bordered,
        zoomed,
      }),
    [color, radius, size, bordered, zoomed]
  )

  return {
    slots,
  }
}

export type UseAvatarReturn = ReturnType<typeof useAvatar>
