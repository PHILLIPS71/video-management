import type { VariantProps } from 'tailwind-variants'

import { tv } from 'tailwind-variants'

const avatar = tv({
  slots: {
    group: ['flex', 'items-center'],
    base: ['flex relative justify-center items-center box-border overflow-hidden align-middle dark:text-white z-10'],
    img: ['flex', 'object-cover', 'w-full', 'h-full'],
    icon: ['flex', 'items-center', 'justify-center', 'text-inherit', 'w-full', 'h-full'],
  },
  variants: {
    size: {
      xs: 'w-6 h-6 text-xs',
      sm: 'w-8 h-8 text-xs',
      md: 'w-12 h-12 text-xs',
      lg: 'w-16 h-16 text-sm',
      xl: 'w-24 h-24 text-md',
    },
    color: {
      neutral: 'bg-zinc-300 text-white',
      primary: 'bg-blue-500 text-white',
      secondary: 'bg-purple-500 text-white',
      success: 'bg-green-500 text-white',
      warning: 'bg-yellow-500 text-white',
      danger: 'bg-red-500 text-white',
    },
    radius: {
      none: 'rounded-none',
      base: 'rounded',
      sm: 'rounded-sm',
      md: 'rounded-md',
      lg: 'rounded-lg',
      xl: 'rounded-xl',
      full: 'rounded-full',
    },
    bordered: {
      true: {
        base: 'ring-2 ring-offset-2',
      },
    },
    zoomed: {
      true: {
        img: 'hover:scale-125 ease-in duration-200',
        icon: 'hover:scale-125 ease-in duration-200',
      },
    },
  },
  compoundVariants: [
    {
      color: 'neutral',
      bordered: true,
      class: {
        base: 'ring-zinc-300',
      },
    },
    {
      color: 'primary',
      bordered: true,
      class: {
        base: 'ring-blue-500',
      },
    },
    {
      color: 'secondary',
      bordered: true,
      class: {
        base: 'ring-purple-500',
      },
    },
    {
      color: 'success',
      bordered: true,
      class: {
        base: 'ring-green-500',
      },
    },
    {
      color: 'warning',
      bordered: true,
      class: {
        base: 'ring-yellow-500',
      },
    },
    {
      color: 'danger',
      bordered: true,
      class: {
        base: 'ring-red-500',
      },
    },
  ],
  defaultVariants: {
    size: 'md',
    color: 'neutral',
    radius: 'full',
  },
})

export type AvatarVariantProps = VariantProps<typeof avatar>
export default avatar
