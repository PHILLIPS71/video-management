import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./src/**/*.{ts,tsx}', './node_modules/@giantnodes/design-system-react/**/*.js'],
  plugins: [],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        backdrop: 'rgb(var(--color-backdrop) / 1)',
        background: 'rgb(var(--color-background) / 1)',
        middleground: 'rgb(var(--color-middleground) / 1)',
        foreground: 'rgb(var(--color-foreground) / 1)',
        title: 'rgb(var(--color-title) / 1)',
        subtitle: 'rgb(var(--color-subtitle) / 1)',
        primary: {
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
        secondary: {
          50: '#eefbfd',
          100: '#d5f2f8',
          200: '#b0e5f1',
          300: '#6ccce4',
          400: '#3cb3d4',
          500: '#2096ba',
          600: '#1e799c',
          700: '#1e6380',
          800: '#215269',
          900: '#204559',
          950: '#102c3c',
          DEFAULT: '#2096ba',
        },
      },
    },
  },
}

export default config
