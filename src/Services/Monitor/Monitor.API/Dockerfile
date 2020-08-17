#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY "Shared/RabbitMqConnectionFactory/RabbitMqConnectionFactory.csproj" "Shared/RabbitMqConnectionFactory/"
COPY "Services/Monitor/Monitor.UnitTests/Monitor.UnitTests.csproj" "Services/Monitor/Monitor.UnitTests/"
COPY "Services/Monitor/Monitor.API/Monitor.API.csproj" "Services/Monitor/Monitor.API/"

RUN dotnet restore "Services/Monitor/Monitor.API/Monitor.API.csproj"
COPY . .

FROM build as publish
WORKDIR "Services/Monitor/Monitor.API"
RUN dotnet publish "Monitor.API.csproj" -c Release -o /app/publish

FROM build AS unittest
WORKDIR "Services/Monitor/Monitor.UnitTests"
RUN dotnet test "Monitor.UnitTests.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitor.API.dll"]
