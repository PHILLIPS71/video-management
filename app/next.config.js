/** @type {import('next').NextConfig} */
const config = {
  reactStrictMode: true,
  compiler: {
    relay: {
      src: './src',
      language: 'typescript',
      artifactDirectory: 'src/__generated__',
    },
  },
}

module.exports = config
