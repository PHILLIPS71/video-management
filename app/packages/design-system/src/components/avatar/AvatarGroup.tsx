import type { AvatarProps } from '@/components/avatar/Avatar'
import type { UseAvatarProps } from '@/components/avatar/use-avatar.hook'
import type { HTMLProps } from '@/utils/system'

import clsx from 'clsx'
import React from 'react'

import { useAvatar } from '@/components/avatar/use-avatar.hook'

export type AvatarGroupProps = HTMLProps<'div', UseAvatarProps> & {
  ref?: React.Ref<HTMLDivElement | null>
}

const AvatarGroup = React.forwardRef<HTMLDivElement, AvatarGroupProps>((props, ref) => {
  const { as, children, className, color, radius, size, bordered, ...rest } = props
  const { slots } = useAvatar(props)

  const Component = as || 'div'

  const getAvatarGroupProps = React.useCallback(
    () => ({
      ref,
      className: slots.group({
        class: className,
      }),
      ...rest,
    }),
    [ref, slots, className, rest]
  )

  return (
    <Component {...getAvatarGroupProps()}>
      {React.Children.map(children, (child, index) => {
        if (!React.isValidElement<AvatarProps>(child)) {
          return child
        }

        return React.cloneElement(child, {
          className: clsx(index !== 0 ? '-ml-2' : 'ml-0'),
          color: color ?? child.props.color,
          radius: radius ?? child.props.radius,
          size: size ?? child.props.size,
          bordered: bordered ?? child.props.bordered,
        })
      })}
    </Component>
  )
})

AvatarGroup.defaultProps = {
  ref: null,
}

export default AvatarGroup
