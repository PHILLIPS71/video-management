import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./src/**/*.{ts,tsx}', './node_modules/@giantnodes/design-system-react/**/*.js'],
  plugins: [],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        bunker: {
          '50': '#f3f4f4',
          '100': '#e8e8e9',
          '200': '#c5c6c8',
          '300': '#a2a4a6',
          '400': '#5d5f64',
          '500': '#171b21',
          '600': '#15181e',
          '700': '#111419',
          '800': '#0e1014',
          '900': '#0b0d10',
          DEFAULT: '#171B21',
        },
        mineshaft: {
          50: '#f5f5f5',
          100: '#ebebeb',
          200: '#ccccce',
          300: '#adaeb0',
          400: '#707174',
          500: '#323439',
          600: '#2d2f33',
          700: '#26272b',
          800: '#1e1f22',
          900: '#19191c',
          DEFAULT: '#323439',
        },
      },
    },
  },
}

export default config
