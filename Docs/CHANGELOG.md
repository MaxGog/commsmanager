# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - ####-##-##

### Added
- Initial project structure (Core, Infrastructure, Application, API, Web, MAUI)
- Docker Compose support for local development (SQL Server, API, Web)
- Dockerfiles for API and Web with multi-stage builds
- Domain entities: `Order`, `Customer`, `ArtistProfile`, `Commission`
- Repository pattern and Unit of Work
- Entity Framework Core with SQL Server
- Basic CRUD operations for customers and orders
- Healthchecks for SQL Server in Docker
```