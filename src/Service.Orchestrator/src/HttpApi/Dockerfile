FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 5623

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src .
RUN dotnet restore "Service.Orchestrator/src/HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj"
RUN dotnet build "Service.Orchestrator/src/HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Orchestrator/src/HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS 5623
ENTRYPOINT ["dotnet", "Giantnodes.Service.Orchestrator.HttpApi.dll"]