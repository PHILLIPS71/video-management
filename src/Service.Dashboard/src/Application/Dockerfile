FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Giantnodes.Service.Dashboard.Application/Giantnodes.Service.Dashboard.Application.csproj", "Giantnodes.Service.Dashboard.Application/"]
RUN dotnet restore "Giantnodes.Service.Dashboard.Application/Giantnodes.Service.Dashboard.Application.csproj"
COPY . .
WORKDIR "/src/Giantnodes.Service.Dashboard.Application"
RUN dotnet build "Giantnodes.Service.Dashboard.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Giantnodes.Service.Dashboard.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Giantnodes.Service.Dashboard.Application.dll"]
