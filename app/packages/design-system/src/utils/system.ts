export type As<Props = any> = React.ElementType<Props>

export type Props = {
  as?: As
}

export type PropsOf<T extends As> = React.ComponentPropsWithoutRef<T> & Props

export type HTMLProps<T extends As = 'div', K extends object = {}> = Omit<
  Omit<PropsOf<T>, 'ref' | 'color' | 'slot'> & Props,
  keyof K
> &
  K
