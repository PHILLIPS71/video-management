import React from 'react'

type ScrollAnchorProps = React.PropsWithChildren

const ScrollAnchor: React.FC<ScrollAnchorProps> = ({ children }) => {
  const ref = React.useRef<HTMLDivElement | null>(null)

  React.useEffect(() => ref.current?.scrollIntoView({ behavior: 'smooth' }))

  return (
    <>
      {children}
      <div ref={ref} id="anchor" />
    </>
  )
}

export default ScrollAnchor
