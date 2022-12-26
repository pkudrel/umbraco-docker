# Umbraco 11 Docker Image


Exmaple `docker-compose.yml` file
```yaml
version: '3.7'

services:
  umbraco-service:
    container_name: umbraco-page
    image: ghcr.io/pkudrel/umbraco-docker:1.0
    restart: always
    entrypoint: [ "dotnet", "Page.dll" ]
    networks:
      - nginx-proxy-manager
    volumes:
     - ./data/sys/logs:/output/umbraco/Logs
     - ./data/sys/temp:/output/umbraco/Data/TEMP
     - ./data/usr/db:/output/umbraco/Data/db
     - ./data/usr/views:/output/Views
     - ./data/usr/media:/output/wwwroot/media
     - ./data/usr/misc/favicon.ico:/output/wwwroot/favicon.ico
     - ./data/etc/appsettings.json:/output/appsettings.json
networks:
  nginx-proxy-manager:
    external: true

```

SQLite connection string 
(one different from default value: Database is in `db` directory)

```json
"ConnectionStrings": {
    "umbracoDbDSN": "Data Source=|DataDirectory|/db/Umbraco.sqlite.db;Cache=Shared;Foreign Keys=True;Pooling=True",
    "umbracoDbDSN_ProviderName": "Microsoft.Data.Sqlite"
  }
```