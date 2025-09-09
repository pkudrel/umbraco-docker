# AGENTS.md

This file provides guidance to LLM AGENTS (e.g. Claude Code - claude.ai/code) when working with code in this repository.


## Project Architecture

This is a **Umbraco CMS 16.2.0** project running on **.NET 9.0**, designed to be deployed via Docker containers.

### Key Components

- **Main Application**: `src/UmbracoPage/` - ASP.NET Core web application with Umbraco CMS
- **Entry Point**: `src/UmbracoPage/Program.cs` - Standard Umbraco bootstrapping with BackOffice and Website setup
- **Solution**: `src/UmbracoDocker.sln` - Single project solution
- **Docker**: Multi-stage Dockerfile for production deployment

### Architecture Patterns

- **Umbraco Standard Setup**: Uses the standard Umbraco builder pattern with BackOffice, Website, and Composers
- **Docker Multi-stage Build**: Build stage with SDK, publish stage, and final runtime stage
- **Port Configuration**: Exposes port 8080 in container, local development uses ports 37160 (HTTP) and 44314 (HTTPS)

## Development Commands

### Local Development
```bash
# Build and run locally
cd src/UmbracoPage
dotnet restore
dotnet build
dotnet run

# Development with hot reload
dotnet watch run
```

### Docker Commands
```bash
# Build Docker image
docker build -t umbraco-page .

# Run container
docker run -p 8080:8080 umbraco-page
```

### Solution Management
```bash
# Build entire solution
dotnet build src/UmbracoDocker.sln

# Clean solution
dotnet clean src/UmbracoDocker.sln

# Restore packages for solution
dotnet restore src/UmbracoDocker.sln
```

## Configuration Notes

- **Target Framework**: .NET 9.0
- **Umbraco Version**: 16.2.0
- **Security**: Concurrent logins disabled, unattended upgrades enabled
- **Content**: Allows editing invariant content from non-default languages
- **Performance**: Compression disabled for Umbraco backoffice files
- **Globalization**: Uses app-local ICU for consistent globalization across platforms

## CI/CD

- **GitHub Actions**: Automated Docker image builds on semver tags (`v*.*.*`)
- **Registry**: Images pushed to GitHub Container Registry (ghcr.io)
- **Build Context**: Root directory (includes Dockerfile and source)