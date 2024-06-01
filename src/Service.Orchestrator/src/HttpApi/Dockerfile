FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Giantnodes.Service.Orchestrator.HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj", "src/Giantnodes.Service.Orchestrator.HttpApi/"]
RUN dotnet restore "src/Giantnodes.Service.Orchestrator.HttpApi/Giantnodes.Service.Orchestrator.HttpApi.csproj"
COPY . .
WORKDIR "/src/src/Giantnodes.Service.Orchestrator.HttpApi"
RUN dotnet build "Giantnodes.Service.Orchestrator.HttpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Giantnodes.Service.Orchestrator.HttpApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Giantnodes.Service.Orchestrator.HttpApi.dll"]
