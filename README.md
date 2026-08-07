# CommsManager 🎨📱

**Professional order management system for creative professionals**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MAUI](https://img.shields.io/badge/MAUI-Blazor_Hybrid-0078D4?logo=xamarin)](https://learn.microsoft.com/dotnet/maui/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-5C2D91?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](https://www.docker.com/)
[![Code Style](https://img.shields.io/badge/code_style-C%23_12-239120?logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)

---

## 🌟 About the Project

CommsManager is a cross-platform solution for artists, crafters, photographers, and other creative professionals that helps manage orders, clients, and showcase works through a personalized landing page.

**Key features:**

- 📋 Order management with status tracking
- 👥 Client database and interaction history
- 🖼️ Portfolio with work samples
- 💰 Flexible price list system
- 🌐 Personal landing page (like Linktree)
- 📱 QR code for quick profile access
- 🔄 Online/offline synchronization
- 📊 Analytics and reporting

---

## 🏗️ Architecture

The project is built on **Domain-Driven Design (DDD)** and **Clean Architecture** principles. Detailed description of layers and patterns can be found in [ARCHITECTURE.md](Docs/ARCHITECTURE.md).

Brief structure:

```
CommsManager/
├── CommsManager.Core/          # Domain layer (entities, VO, interfaces)
├── CommsManager.Infrastructure/# Infrastructure (EF Core, repositories)
├── CommsManager.Application/   # Use cases (CQRS, MediatR)
├── CommsManager.API/           # REST API on ASP.NET Core
├── CommsManager.Web/           # Blazor WebAssembly client
├── CommsManager.Maui/          # MAUI Hybrid (mobile app)
├── CommsManager.Shared/        # Shared components (UI, models)
├── docker-compose.yml          # Container configuration
└── .env                        # Environment variables template
```

---

## 🚀 Quick Start

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recommended) or Docker Engine + Compose
- (optional) [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) for local development

### Run with Docker (recommended)

1. **Clone the repository:**
   ```bash
   git clone https://github.com/MaxGog/CommsManager.git
   cd CommsManager
   ```

2. **Set up environment variables:**
   Copy `.env.example` to `.env` and set a password for SQL Server and (optionally) a Jwt key for development:
   ```bash
   cp .env.example .env
   # Edit .env, set DB_PASSWORD=YourStrong!Passw0rd
   # OPTIONAL for local dev: set JWT_KEY to a secure 32+ byte value
   ```

   Important: do NOT commit secrets (Jwt key, DB passwords) to the repository.

3. **Start all services:**
   ```bash
   docker-compose up -d --build
   ```
   This will spin up:
   - **SQL Server** on port `1433`
   - **API** on port `5000`
   - **Web** on port `5001`

4. **Verify the setup:**
   - Web UI: [http://localhost:5001](http://localhost:5001)
   - API Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)

5. **Stop the services:**
   ```bash
   docker-compose down
   ```

### Local Development (without Docker)

1. Install .NET 10 SDK.
2. Restore dependencies and build the solution:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Configure the database connection string in `appsettings.json` for each project (or use `User Secrets`).
4. Apply migrations:
   ```bash
   dotnet ef database update --project CommsManager.Infrastructure
   ```
5. Run API and Web separately:
   ```bash
   dotnet run --project CommsManager.API
   dotnet run --project CommsManager.Web
   ```

---

## 🧭 Authentication & Web UI

This repository includes a simple authentication flow (aligned with Clean Architecture) with two user roles:

- `Client` — default role when a user registers.
- `Creator` — a promoted role. Clients can be promoted to Creator; creators cannot be downgraded back to Client (business rule). A Creator may still act as a Client for other Creators' interactions where business logic allows it.

Implemented endpoints (API):

- `POST /api/auth/register` — register (returns JWT token)
- `POST /api/auth/login` — login (returns JWT token)
- `POST /api/auth/promote` — promote current user to Creator (requires authentication)

Web UI (manual testing):

- `/register` — register a new user. After success the typed HttpClient sets Authorization header.
- `/login` — login and set Authorization header for subsequent requests.

JWT configuration:
- The API reads the following settings from configuration / environment: `Jwt:Key`, `Jwt:Issuer`, `Jwt:ExpiryMinutes`.
- For HS256, Jwt:Key must be sufficiently long (at least 32 bytes / 256 bits). For development you can set JWT_KEY in `.env` and wire it into the API container environment.

Example (use token in Swagger):
1. Register via Web or `POST /api/auth/register`.
2. Copy returned token and click "Authorize" in Swagger (bearer token).
3. Call protected endpoints (e.g., `GET /api/test/me`).

---

## 🗄️ Migrations & Database

The project uses EF Core migrations (located in `CommsManager.Infrastructure/Migrations`).

If developing on macOS/Linux where LocalDB is not available, apply migrations to the SQL Server running in Docker using an explicit connection string:

```bash
# Example (when Docker Compose is running and DB_PASSWORD is set in .env)
export DB_PASSWORD=YourStrong!Passw0rd
dotnet ef database update --project CommsManager.Infrastructure --connection "Server=localhost,1433;Database=CommsManagerDb;User=sa;Password=${DB_PASSWORD};TrustServerCertificate=True;"
```

Alternatively, run migrations from inside a container that has dotnet SDK installed and network access to the sqlserver container.

---

## 🧪 Testing

Run all tests in the repository:

```bash
dotnet test
```

Or run a specific test project for speed (auth unit tests):

```bash
dotnet test tests/CommsManager.Application.Tests
```

Notes:
- Authentication unit tests are in `tests/CommsManager.Application.Tests` and validate register/login flows.
- When running tests locally, the test configuration sets a long-enough Jwt key. Ensure any custom test config follows the same requirement.

---

## 🤝 How to Contribute

We welcome contributions! Please read [CONTRIBUTING.md](CONTRIBUTING.md) to learn about commit message guidelines, pull request process, and code style.

**Quick checklist:**
- Fork the repository and create a branch for your changes.
- Follow [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.
- Ensure all tests pass.
- Update documentation if needed.

---

## 📄 License & Contact

This project is distributed under the MIT license. See [LICENSE](LICENSE) for details.

- **Author:** Maxim Goglov — [max.gog2005@outlook.com](mailto:max.gog2005@outlook.com)
- **Report an issue:** [GitHub Issues](https://github.com/MaxGog/CommsManager/issues)

---

## 🙏 Acknowledgements

Thanks to everyone who participates in the development of the project!  
If you like the project, give it a ⭐ on GitHub – it helps us grow.