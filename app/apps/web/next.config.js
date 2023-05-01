/** @type {import('next').NextConfig} */
const config = {
  reactStrictMode: true,
  swcMinify: true,
  compiler: {
    relay: {
      src: './src',
      language: 'typescript',
      artifactDirectory: '__generated__',
    },
  },
  experimental: {
    appDir: true,
    typedRoutes: true,
  },
}

module.exports = config
