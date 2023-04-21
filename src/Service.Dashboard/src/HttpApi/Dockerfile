FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Giantnodes.Service.Dashboard.HttpApi/Giantnodes.Service.Dashboard.HttpApi.csproj", "src/Giantnodes.Service.Dashboard.HttpApi/"]
RUN dotnet restore "src/Giantnodes.Service.Dashboard.HttpApi/Giantnodes.Service.Dashboard.HttpApi.csproj"
COPY . .
WORKDIR "/src/src/Giantnodes.Service.Dashboard.HttpApi"
RUN dotnet build "Giantnodes.Service.Dashboard.HttpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Giantnodes.Service.Dashboard.HttpApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Giantnodes.Service.Dashboard.HttpApi.dll"]
