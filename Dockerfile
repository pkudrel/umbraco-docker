FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG VERSION=unknown
ARG BUILD_DATE=unknown
ARG VCS_REF=unknown
WORKDIR /src
COPY ["src/UmbracoPage/UmbracoPage.csproj", "UmbracoPage/"]
RUN dotnet restore "UmbracoPage/UmbracoPage.csproj"
COPY ["src/UmbracoPage/", "UmbracoPage/"]
WORKDIR "/src/UmbracoPage"
RUN dotnet build "UmbracoPage.csproj" -c  $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "UmbracoPage.csproj" -c  $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG VERSION=unknown
ARG BUILD_DATE=unknown
ARG VCS_REF=unknown

LABEL org.opencontainers.image.version="${VERSION}" \
      org.opencontainers.image.created="${BUILD_DATE}" \
      org.opencontainers.image.revision="${VCS_REF}" \
      org.opencontainers.image.title="Umbraco Docker" \
      org.opencontainers.image.description="Umbraco CMS Docker container"

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UmbracoPage.dll"]