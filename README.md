# Umbraco Docker

Docker container for Umbraco CMS v16.2.0 built on .NET 9.0.

## Quick Start

```bash
# Pull the latest image
docker pull ghcr.io/pkudrel/umbraco-docker:latest

# Run the container
docker run -p 8080:8080 ghcr.io/pkudrel/umbraco-docker:latest
```

Access Umbraco at `http://localhost:8080`

## Image Tags

- `latest` - Latest production build
- `16.2.x` - Semantic version (auto-generated)
- `16.2` - Major.minor version
- `16` - Major version only

## Building Locally

```bash
# Build the image
docker build -t umbraco-docker .

# Run locally
docker run -p 8080:8080 umbraco-docker
```

## Project Structure

```
├── src/
│   └── UmbracoPage/        # Umbraco CMS application
├── .github/
│   ├── actions/
│   │   └── semver-js/      # Custom semantic versioning action
│   └── workflows/
│       └── docker-image.yml # Docker build & publish workflow
├── Dockerfile              # Multi-stage Docker build
└── version.txt            # Base version for semantic versioning
```

## Versioning

This project uses automated semantic versioning:

- **Base version**: Defined in `version.txt` (currently 16.2.0)
- **Patch increment**: Automatically calculated from commits since base version change
- **Generated tags**: `16.2.x`, `16.2`, `16`, `latest`

Example: With 9 commits since `version.txt` was set to `16.2.0`, the generated version is `16.2.9`.

## Development

### Requirements

- .NET 9.0 SDK
- Docker
- Git

### Local Development

```bash
# Restore packages
dotnet restore src/UmbracoDocker.sln

# Build project
dotnet build src/UmbracoDocker.sln

# Run locally (development)
cd src/UmbracoPage
dotnet run
```

### CI/CD

Images are automatically built and published to GitHub Container Registry when pushing to the `production` branch.

The build process:
1. Generates semantic version from commits
2. Builds multi-stage Docker image
3. Injects version metadata as OCI labels
4. Publishes to `ghcr.io/pkudrel/umbraco-docker`

## Configuration

The Umbraco application can be configured through:
- Environment variables
- `appsettings.json` modifications
- Volume mounts for persistent data

## Exmaple `docker-compose.yml` file
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
     - ./data/usr/usync:/output/uSync
     - ./data/usr/media:/output/wwwroot/media
     - ./data/usr/misc/favicon.ico:/output/wwwroot/favicon.ico
     - ./data/etc/appsettings.json:/output/appsettings.json
networks:
  nginx-proxy-manager:
    external: true

```
## License

[License details in LICENSE.md](LICENSE.md)