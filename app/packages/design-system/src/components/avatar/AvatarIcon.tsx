import type { UseAvatarProps } from '@/components/avatar/use-avatar.hook'
import type { HTMLProps } from '@/utils/system'

import React from 'react'

import { useAvatar } from '@/components/avatar/use-avatar.hook'

export type AvatarIconProps = Omit<HTMLProps<'span', UseAvatarProps>, 'children'> & {
  ref?: React.Ref<HTMLSpanElement | null>
  icon: React.ReactNode
}

const AvatarIcon = React.forwardRef<HTMLImageElement, AvatarIconProps>((props, ref) => {
  const { as, className, icon, ...rest } = props
  const { slots } = useAvatar(props)

  const Component = as || 'span'

  const getAvatarIconProps = React.useCallback(
    () => ({
      ref,
      className: slots.icon({
        class: className,
      }),
      ...rest,
    }),
    [ref, slots, className, rest]
  )

  return <Component {...getAvatarIconProps()}>{icon}</Component>
})

AvatarIcon.defaultProps = {
  ref: null,
}

export default AvatarIcon
