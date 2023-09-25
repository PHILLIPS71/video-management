/** @type {import('next').NextConfig} */
const config = {
  reactStrictMode: true,
  swcMinify: true,
  compiler: {
    relay: {
      src: './src',
      language: 'typescript',
      artifactDirectory: 'src/__generated__',
    },
  },
  experimental: {
    typedRoutes: true,
  },
}

module.exports = config
