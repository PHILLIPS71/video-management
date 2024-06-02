import type { Config } from 'tailwindcss'
import ReactAriaComponents from 'tailwindcss-react-aria-components'
import { giantnodes } from '@giantnodes/theme'

const config: Config = {
  content: [
    './src/**/*.{ts,tsx}',
    './node_modules/@giantnodes/theme/dist/**/*.{js,mjs,cjs}',
    './node_modules/@giantnodes/react/dist/**/*.{js,mjs,cjs}',
  ],
  plugins: [giantnodes(), ReactAriaComponents()],
  darkMode: 'class',
}

export default config
