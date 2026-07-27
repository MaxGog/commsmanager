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
   Copy `.env.example` to `.env` and set a password for SQL Server:
   ```bash
   cp .env.example .env
   # Edit .env, set DB_PASSWORD=YourStrong!Passw0rd
   ```

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

## 🧪 Testing

The project is covered with unit and integration tests. Run them with:
```bash
dotnet test
```

---

## 🤝 How to Contribute

We welcome any contributions! Please read [CONTRIBUTING.md](CONTRIBUTING.md) to learn about commit message guidelines, pull request process, and code style.

**Quick checklist:**
- Fork the repository and create a branch for your changes.
- Follow [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.
- Ensure all tests pass.
- Update documentation if needed.

---

## 📄 License

This project is distributed under the MIT license. See [LICENSE](LICENSE) for details.

---

## 📞 Contact & Support

- **Author:** Maxim Goglov
- **Email:** [max.gog2005@outlook.com](mailto:max.gog2005@outlook.com)
- **Telegram:** [@maxgog](https://t.me/maxgog)
- **Report an issue:** [GitHub Issues](https://github.com/MaxGog/CommsManager/issues)
- **Discussions:** [GitHub Discussions](https://github.com/MaxGog/CommsManager/discussions)

---

## 🙏 Acknowledgements

Thanks to everyone who participates in the development of the project!  
If you like the project, give it a ⭐ on GitHub – it helps us grow.