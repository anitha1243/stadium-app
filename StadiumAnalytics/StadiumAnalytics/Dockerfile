FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/StadiumAnalytics.Api/StadiumAnalytics.Api.csproj", "src/StadiumAnalytics.Api/"]
COPY ["src/StadiumAnalytics.Infrastructure/StadiumAnalytics.Infrastructure.csproj", "src/StadiumAnalytics.Infrastructure/"]
RUN dotnet restore "src/StadiumAnalytics.Api/StadiumAnalytics.Api.csproj"
COPY . .
WORKDIR "/src/src/StadiumAnalytics.Api"
RUN dotnet build "StadiumAnalytics.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StadiumAnalytics.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StadiumAnalytics.Api.dll"]