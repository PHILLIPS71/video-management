import type { UseAvatarProps } from '@/components/avatar/use-avatar.hook'
import type { HTMLProps } from '@/utils/system'

import React from 'react'

import { useAvatar } from '@/components/avatar/use-avatar.hook'

export type AvatarImageProps = HTMLProps<'img', UseAvatarProps> & {
  ref?: React.Ref<HTMLImageElement | null>
  src: string
  alt: string
}

const AvatarImage = React.forwardRef<HTMLImageElement, AvatarImageProps>((props, ref) => {
  const { as, children, className, src, alt, ...rest } = props
  const { slots } = useAvatar(props)

  const Component = as || 'img'

  const getAvatarImageProps = React.useCallback(
    () => ({
      ref,
      src,
      alt,
      className: slots.img({
        class: className,
      }),
      ...rest,
    }),
    [ref, src, alt, slots, className, rest]
  )

  return <Component {...getAvatarImageProps()}>{children}</Component>
})

AvatarImage.defaultProps = {
  ref: null,
}

export default AvatarImage
