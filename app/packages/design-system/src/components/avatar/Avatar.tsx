import type { UseAvatarProps } from '@/components/avatar/use-avatar.hook'
import type { HTMLProps } from '@/utils/system'

import React from 'react'

import AvatarGroup from '@/components/avatar/AvatarGroup'
import AvatarIcon from '@/components/avatar/AvatarIcon'
import AvatarImage from '@/components/avatar/AvatarImage'
import { useAvatar } from '@/components/avatar/use-avatar.hook'

export type AvatarProps = HTMLProps<'span', UseAvatarProps> & {
  ref?: React.Ref<HTMLSpanElement | null>
}

const Avatar = React.forwardRef<HTMLSpanElement, AvatarProps>((props, ref) => {
  const { as, className, children, ...rest } = props
  const { slots } = useAvatar(props)

  const Component = as || 'span'

  const getAvatarProps = React.useCallback(
    () => ({
      ref,
      className: slots.base({
        class: className,
      }),
      ...rest,
    }),
    [ref, slots, className, rest]
  )

  return <Component {...getAvatarProps()}>{children}</Component>
})

Avatar.defaultProps = {
  ref: null,
}

export default Object.assign(Avatar, {
  Group: AvatarGroup,
  Image: AvatarImage,
  Icon: AvatarIcon,
})
