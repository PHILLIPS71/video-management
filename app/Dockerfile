FROM node:20-alpine AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS exporter
WORKDIR /app
COPY src .
RUN dotnet run --project "Service.Orchestrator/src/HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj" -- schema export --output /app/schema.graphql

FROM base AS installer
COPY /app/package.json /app/pnpm-lock.yaml* ./
RUN corepack enable pnpm && pnpm i --frozen-lockfile

FROM base AS builder
COPY app .
COPY --from=installer /app/node_modules ./node_modules
COPY --from=exporter /app/schema.graphql ./
ENV NODE_ENV production
RUN corepack enable pnpm && pnpm run build

FROM base AS runner
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static
ENV PORT 3000
ENV NODE_ENV production
ENV NEXT_PUBLIC_API_URI http://localhost:5263
CMD HOSTNAME="0.0.0.0" node server.js