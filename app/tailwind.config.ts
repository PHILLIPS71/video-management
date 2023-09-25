import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./src/**/*.{ts,tsx}', './node_modules/@giantnodes/theme/dist/**/*.{js,cjs}'],
  plugins: [],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        title: 'rgb(var(--color-title) / 1)',
        subtitle: 'rgb(var(--color-subtitle) / 1)',
        brand: {
          50: '#f1fcf4',
          100: '#defae8',
          200: '#bef4d3',
          300: '#8beab0',
          400: '#51d786',
          500: '#2cc76a',
          600: '#1d9c50',
          700: '#1a7b41',
          800: '#1a6137',
          900: '#175030',
          950: '#072c17',
          DEFAULT: '#2cc76a',
        },
        shark: {
          50: '#f4f4f5',
          100: '#e9eaea',
          200: '#c8cacc',
          300: '#a7aaad',
          400: '#656a6f',
          500: '#232a31',
          600: '#20262c',
          700: '#1a2025',
          800: '#15191d',
          900: '#111518',
          950: '#0e1114',
          DEFAULT: '#232a31',
        },
      },
    },
  },
}

export default config
