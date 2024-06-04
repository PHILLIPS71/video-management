FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 5600

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src .
RUN dotnet restore "Service.Encoder/src/Console/Giantnodes.Service.Encoder.Console.csproj"
RUN dotnet build "Service.Encoder/src/Console/Giantnodes.Service.Encoder.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Encoder/src/Console/Giantnodes.Service.Encoder.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS 5600
ENTRYPOINT ["dotnet", "Giantnodes.Service.Encoder.Console.dll"]