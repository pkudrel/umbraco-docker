# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /sources

# Copy everything else and build website
COPY /src/Page/*.csproj .

COPY /src/Page/appsettings.json .
COPY /src/Page/Program.cs .
COPY /src/Page/Startup.cs .

COPY /src/Page/Properties ./Properties
COPY /src/Page/Views ./Views

# Build umbraco
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c release -o /output --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /output
COPY --from=build /output ./


ENTRYPOINT ["dotnet", "Page.dll"]