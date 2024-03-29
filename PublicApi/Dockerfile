#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80/tcp
#
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["PublicApi/PublicApi.csproj", "PublicApi/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["ApplicationCore/ApplicationCore.csproj", "ApplicationCore/"]
RUN dotnet restore "PublicApi/PublicApi.csproj"
COPY . .
WORKDIR "/PublicApi"
RUN dotnet build "PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApi.dll"]
