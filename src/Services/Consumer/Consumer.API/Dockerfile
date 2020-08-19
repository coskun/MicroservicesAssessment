#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY "Services/Monitor/Monitor.UnitTests/Monitor.UnitTests.csproj" "Services/Monitor/Monitor.UnitTests/"
COPY "Services/Monitor/Monitor.API/Monitor.API.csproj" "Services/Monitor/Monitor.API/"
COPY "Services/Consumer/Consumer.API/Consumer.API.csproj" "Services/Consumer/Consumer.API/"

RUN dotnet restore "Services/Consumer/Consumer.API/Consumer.API.csproj"
COPY . .

FROM build as publish
WORKDIR "Services/Consumer/Consumer.API"
RUN dotnet publish "Consumer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.API.dll"]
