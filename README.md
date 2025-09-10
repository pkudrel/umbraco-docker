# Umbraco Docker

Docker container for Umbraco CMS v16.2.0 built on .NET 9.0.

**Includes additional packages**:
- **uSync Complete v16.0.6** - Content synchronization and deployment

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
├── example-page/           # Example deployment setup
│   ├── app/config/         # Example configuration
│   └── docker-compose.yml  # Ready-to-use deployment
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
5. Creates GitHub release with auto-generated release notes

## Configuration

The Umbraco application can be configured through:
- Environment variables
- `appsettings.json` modifications
- Volume mounts for persistent data

## Example Page Setup

The `example-page/` directory contains a ready-to-use example deployment:

```bash
# Navigate to example setup
cd example-page

# Start the example deployment
docker-compose up -d
```

**Includes**:
- **Pre-configured Umbraco settings** (`app/config/appsettings.json`)
- **Volume mapping** for persistent data and configuration
- **Ready-to-run** Docker Compose setup

The example demonstrates best practices for:
- Configuration management
- Data persistence
- Volume mounting
- Environment setup

### Docker Compose Features

The included `docker-compose.yml` provides:

**Environment Configuration**:
- Custom port mapping (8080)
- Serilog console-only logging
- SQLite database with unattended installation

**Volume Mappings**:
```yaml
volumes:
  - ./app/work/dp-keys:/root/.aspnet/DataProtection-Keys  # ASP.NET data protection
  - ./app/log:/app/umbraco/Logs                          # Application logs
  - ./app/work/data:/app/umbraco/Data                     # Umbraco data files
  - ./app/work/usync:/app/uSync                           # uSync configuration
  - ./app/work/models:/app/umbraco/models                 # Generated models
  - ./app/work/views:/app/Views                           # Custom views
  - ./app/wwwroot/media:/app/wwwroot/media                # Media files
  - ./app/config/appsettings.json:/app/appsettings.json   # Configuration override
```

**Key Features**:
- **Unattended install**: Automatic setup without manual configuration
- **Persistent data**: All important data survives container restarts
- **Custom configuration**: Override settings via volume-mounted appsettings.json
- **Development ready**: Includes logging, views, and media persistence

## License

[License details in LICENSE.md](LICENSE.md)