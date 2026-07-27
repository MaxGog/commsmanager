# CommsManager Project Architecture

## Overall Structure

```
CommsManager/
├── CommsManager.Core/          # Domain layer (business logic)
├── CommsManager.Infrastructure/# Infrastructure layer (implementation)
├── CommsManager.Application/   # Use cases (CQRS, MediatR)
├── CommsManager.Web/           # Web
├── CommsManager.Maui/          # MAUI client
└── CommsManager.Shared/        # Shared code
```

---

## 1. Domain Layer (CommsManager.Core)

### 1.1 Core Entities

| Entity | Inheritance | Key Properties | Collections | Business Methods |
|--------|-------------|----------------|-------------|------------------|
| **Order** | `BaseEntity` | `Title`, `Price (Money)`, `Status`, `Deadline`, `CustomerId`, `ArtistId`, `IsActive`, `CreatedDate` | `Attachments (OrderAttachment[])` | `UpdateStatus()`, `Cancel()`, `Complete()`, `ExtendDeadline()`, `UpdateOrderDetails()` |
| **Customer** | `BaseEntity` | `Name`, `CreatedDate`, `IsActive`, `CustomerPicture`, `Description`, `Communication` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Orders (Order[])` | `AddPhone()`, `RemovePhone()`, `AddEmail()`, `Activate()`, `Deactivate()`, `SetCustomerPicture()` |
| **ArtistProfile** | `BaseEntity` | `Name`, `Description`, `ArtistPicture`, `ArtistBanner`, `CreatedDate` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Commissions (Commission[])` | `SetArtistPicture()`, `SetArtistBanner()`, `AddCommission()`, `UpdateProfile()` |
| **BaseEntity** | - | `Id (Guid)`, `DomainEvents (IDomainEvent[])` | - | `AddDomainEvent()`, `ClearDomainEvents()` |

### 1.2 Data Models

| Model | Purpose | Properties | Usage |
|-------|---------|------------|-------|
| **Commission** | Artist's services | `Name`, `Description`, `ViewAttachment (byte[][])`, `TypeCommission`, `Price` | Used in `ArtistProfile.Commissions` |
| **Email** | Contact information | `EmailAddress`, `TypeEmail`, `Description` | Used in `Customer.Emails` and `ArtistProfile.Emails` |
| **OrderAttachment** | Order attachments | `Name`, `Attachment (byte[])`, `TypeAttachment (AttachmentType)`, `Description` | Used in `Order.Attachments` |
| **Phones** | Phone numbers | `NumberPhone`, `TypePhone`, `RegionNumber`, `Description` | Used in `Customer.Phones` and `ArtistProfile.Phones` |
| **SocialLink** | Social networks | `Link`, `TypeLink (SocialPlatform)`, `IsActive`, `IsVisible` | Used in `Customer.SocialLinks` and `ArtistProfile.SocialLinks` |

### 1.3 Value Objects

| VO | Purpose | Properties | Usage |
|----|---------|------------|-------|
| **Money** | Monetary amount | `Amount (decimal)`, `Currency (string)`, `Symbol (string)` | `Order.Price` |

### 1.4 Interfaces

| Interface | Purpose | Methods |
|-----------|---------|---------|
| **IRepository<T>** | Base repository | `GetByIdAsync()`, `GetAllAsync()`, `FindAsync()`, `AddAsync()`, `UpdateAsync()`, `DeleteAsync()`, `ExistsAsync()`, `CountAsync()` |
| **ICustomerRepository** | Customer repository | `GetActiveCustomersAsync()`, `SearchByNameAsync()`, `HasOrdersAsync()`, `GetCustomersWithOrdersAsync()` |
| **IOrderRepository** | Order repository | `GetByCustomerIdAsync()`, `GetByArtistIdAsync()`, `GetActiveOrdersAsync()`, `GetOverdueOrdersAsync()`, `GetOrdersByStatusAsync()`, `GetTotalRevenueByArtistAsync()` |
| **IArtistProfileRepository** | Artist profile repository | `GetProfileWithCommissionsAsync()`, `SearchByNameAsync()`, `HasActiveCommissionsAsync()`, `GetPopularArtistsAsync()` |
| **IUnitOfWork** | Unit of Work pattern | `Customers`, `Orders`, `ArtistProfiles`, `SaveChangesAsync()`, `BeginTransactionAsync()`, `CommitTransactionAsync()`, `RollbackTransactionAsync()` |
| **ICustomerService** | Customer service | `CreateCustomerAsync()`, `AddCustomerContactAsync()`, `GetCustomerOrdersAsync()`, `DeactivateInactiveCustomersAsync()` |
| **IOrderService** | Order service | `CreateOrderAsync()`, `UpdateOrderStatusAsync()`, `DuplicateOrderAsync()`, `GetOrdersDueSoonAsync()`, `ProcessOverdueOrdersAsync()` |
| **IFileStorageService** | File storage service | `SaveAttachmentAsync()`, `GetAttachmentAsync()`, `DeleteAttachmentAsync()`, `SaveCustomerPictureAsync()`, `SaveArtistPictureAsync()` |
| **ICacheService** | Caching service | `GetAsync()`, `SetAsync()`, `RemoveAsync()`, `ExistsAsync()`, `GetOrSetAsync()` |

### 1.5 Enums

| Enum | Used in | Values |
|------|---------|--------|
| **OrderStatus** | `Order.Status` | `New`, `InProgress`, `Completed`, `Cancelled` |
| **AttachmentType** | `OrderAttachment.TypeAttachment` | `Image`, `Document`, `Audio`, `Video`, `Other` |
| **SocialPlatform** | `SocialLink.TypeLink` | `Telegram`, `VK`, `Instagram`, `Twitter`, `Facebook`, `YouTube`, `Other` |

### 1.6 Relationships Between Entities

#### 1.6.1 Customer ↔ Order

- **Navigation**: `Customer.Orders` → `Order[]`
- **Foreign Key**: `Order.CustomerId` (Guid)
- **Relationship Type**: One-to-Many (1 Customer → N Orders)
- **Delete Behavior**: `DeleteBehavior.Restrict`

#### 1.6.2 ArtistProfile ↔ Order

- **Navigation**: No direct navigation in ArtistProfile
- **Foreign Key**: `Order.ArtistId` (Guid)
- **Relationship Type**: One-to-Many (1 Artist → N Orders)

#### 1.6.3 ArtistProfile ↔ Commission

- **Navigation**: `ArtistProfile.Commissions` → `Commission[]`
- **Foreign Key**: `Commission.ArtistProfileId` (Guid)
- **Relationship Type**: One-to-Many (1 Artist → N Commissions)
- **Delete Behavior**: `DeleteBehavior.Cascade`

---

## 2. Infrastructure Layer (CommsManager.Infrastructure)

### 2.1 Entity Framework Core Configurations

| Configuration | Purpose | Key Settings |
|---------------|---------|--------------|
| **ApplicationDbContext** | Main DbContext | `DbSet<Customer>`, `DbSet<Order>`, `DbSet<ArtistProfile>`, `DbSet<Commission>` |
| **CustomerConfiguration** | Customer configuration | Owned Types: `Phones`, `Emails`, `SocialLinks`. Indexes: `Name`, `IsActive`, `CreatedDate` |
| **OrderConfiguration** | Order configuration | Owned Types: `Attachments`. `Money` conversion. Indexes: `CustomerId`, `ArtistId`, `Status`, `Deadline` |
| **ArtistProfileConfiguration** | ArtistProfile configuration | Owned Types: `Phones`, `Emails`, `SocialLinks`, `Commissions`. JSON serialization for `ViewAttachment` |
| **CommissionConfiguration** | Commission configuration | JSON serialization for `ViewAttachment`. Relationship with `ArtistProfile`. Indexes: `Name`, `TypeCommission`, `ArtistProfileId` |
| **DesignTimeDbContextFactory** | Factory for migrations | Creates DbContext for design-time operations |

### 2.2 Repository Implementations

| Repository | Implements | Features |
|------------|------------|----------|
| **CustomerRepository** | `ICustomerRepository` | Includes related data (`Phones`, `Emails`, `Orders`). Filtering by `IsActive` |
| **OrderRepository** | `IOrderRepository` | Includes `Attachments`. Special queries: `GetOverdueOrdersAsync()`, `GetTotalRevenueByArtistAsync()` |
| **ArtistProfileRepository** | `IArtistProfileRepository` | Includes `Commissions`. Method `GetPopularArtistsAsync()` with sorting by commission count |
| **UnitOfWork** | `IUnitOfWork` | Transaction management. Unit of Work pattern for data consistency |

### 2.3 Database Settings

#### 2.3.1 Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CommsManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

#### 2.3.2 Data Type Mapping

| C# Data Type | SQL Server Type | Notes |
|--------------|-----------------|-------|
| `Guid` | `uniqueidentifier` | Primary keys |
| `DateTime` | `datetime2` | High‑precision date and time |
| `byte[]` | `varbinary(max)` | Images and files |
| `string` | `nvarchar(max)` or `nvarchar(N)` | Strings with specified max length |
| `enum` | `nvarchar(50)` | Stored as strings |
| `List<byte[]>` | `nvarchar(max)` | JSON serialization for complex structures |

#### 2.3.3 Indexes (for performance)

| Table | Indexed Columns | Index Type |
|-------|-----------------|------------|
| **Customers** | `Name`, `IsActive`, `CreatedDate`, `(IsActive, Name)` | Non‑unique |
| **Orders** | `CustomerId`, `ArtistId`, `Status`, `Deadline`, `IsActive` | Non‑unique |
| **ArtistProfiles** | `Name`, `CreatedDate` | Non‑unique |
| **Commissions** | `Name`, `TypeCommission`, `ArtistProfileId` | Non‑unique |

### 2.4 Dependency Injection

#### 2.4.1 Service Registration

```csharp
// In AddInfrastructure method
services.AddDbContext<ApplicationDbContext>(options => ...);
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IArtistProfileRepository, ArtistProfileRepository>();
```

#### 2.4.2 Service Lifetimes

| Service | Lifetime | Reason |
|---------|----------|--------|
| `ApplicationDbContext` | Scoped | One context per HTTP request |
| Repositories | Scoped | Consistency with DbContext |
| `IUnitOfWork` | Scoped | Transaction management within request scope |

---

## 3. Design Principles

### 3.1 Patterns Used

| Pattern | Implementation | Benefits |
|---------|----------------|----------|
| **Repository** | `IRepository<T>`, `CustomerRepository` | Data access abstraction, easy testing |
| **Unit of Work** | `IUnitOfWork`, `UnitOfWork` | Transaction consistency, change tracking |
| **Dependency Injection** | Constructor injection | Flexibility, testability, loose coupling |
| **Value Object** | `Money` | Immutability, semantic meaning |
| **Owned Types** | `Phones`, `Emails` in `Customer` | Grouping related data within owner table |
| **Domain Events** | `IDomainEvent` in `BaseEntity` | Reacting to domain changes |

### 3.2 SOLID Principles

| Principle | Implementation |
|-----------|----------------|
| **Single Responsibility** | Each class has one reason to change |
| **Open/Closed** | Extension through new interface implementations |
| **Liskov Substitution** | Inheritance from `BaseEntity`, implementation of `IRepository<T>` |
| **Interface Segregation** | Specialized repository interfaces |
| **Dependency Inversion** | Depend on abstractions (`IRepository`), not implementations |

### 3.3 Domain‑Driven Design (DDD) Principles

| DDD Concept | Implementation |
|-------------|----------------|
| **Aggregate Root** | `Customer`, `Order`, `ArtistProfile` as aggregate roots |
| **Value Objects** | `Money` as an immutable object |
| **Entities** | Entities with identity (`Id`) |
| **Repositories** | Abstractions for aggregate persistence |
| **Domain Events** | Events in `BaseEntity.DomainEvents` |

---

## 4. Database Migrations

### 4.1 Creating Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName --project CommsManager.Infrastructure

# Update the database
dotnet ef database update --project CommsManager.Infrastructure

# Rollback a migration
dotnet ef migrations remove --project CommsManager.Infrastructure
```

### 4.2 Existing Migrations

| Migration | Changes |
|-----------|---------|
| `InitialCreate` | Created tables: Customers, Orders, ArtistProfiles, Commissions |
| `FixCommissionEntity` | Fixed Commission configuration, added primary key |

Отличная идея! Добавлю в `ARCHITECTURE.md` полноценный раздел о Docker, который описывает контейнеризацию, взаимодействие сервисов и настройку окружения. Также исправлю небольшую неточность в `README.md` (путь к `.env`).

---

## 5. Docker Containerization

CommsManager uses Docker to provide a consistent, production-like environment for local development and deployment. The setup includes three main services: SQL Server, API, and Web.

### 5.1 Service Overview

| Service | Container | Port (host) | Description |
|---------|-----------|-------------|-------------|
| **SQL Server** | `mcr.microsoft.com/mssql/server:2022-latest` | `1433` | Relational database for the application |
| **API** | Custom build (CommsManager.API) | `5000` | ASP.NET Core REST API |
| **Web** | Custom build (CommsManager.Web) | `5001` | Blazor WebAssembly frontend |

### 5.2 Docker Compose Configuration

The `docker-compose.yml` file orchestrates all services:

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: ${DB_PASSWORD}
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sql_data:/var/opt/mssql
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${DB_PASSWORD} -Q "SELECT 1" || exit 1
      interval: 10s
      timeout: 5s
      retries: 5

  api:
    build:
      context: .
      dockerfile: CommsManager.API/Dockerfile
    ports:
      - "5000:80"
    depends_on:
      sqlserver:
        condition: service_healthy
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=CommsManagerDb;User=sa;Password=${DB_PASSWORD};TrustServerCertificate=True

  web:
    build:
      context: .
      dockerfile: CommsManager.Web/Dockerfile
    ports:
      - "5001:80"
    depends_on:
      - api
    environment:
      - ApiBaseUrl=http://api:5000

volumes:
  sql_data:
```

### 5.3 Dockerfiles

Each service has its own `Dockerfile` that defines how the container is built.

#### 5.3.1 API Dockerfile (CommsManager.API/Dockerfile)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy and restore all project files (optimized layer caching)
COPY CommsManager.API/*.csproj CommsManager.API/
COPY CommsManager.Application/*.csproj CommsManager.Application/
COPY CommsManager.Core/*.csproj CommsManager.Core/
COPY CommsManager.Infrastructure/*.csproj CommsManager.Infrastructure/

RUN dotnet restore CommsManager.API/CommsManager.API.csproj

# Copy all source code and publish
COPY . .
WORKDIR /src/CommsManager.API
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CommsManager.API.dll"]
```

**Key features:**
- Multi‑stage build (build + runtime) for smaller images
- Optimized layer caching by copying `.csproj` files before source code
- .NET 10 SDK and runtime images from Microsoft Container Registry

#### 5.3.2 Web Dockerfile (CommsManager.Web/Dockerfile)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy and restore project files (Web + Shared)
COPY CommsManager.Web/*.csproj CommsManager.Web/
COPY CommsManager.Shared/*.csproj CommsManager.Shared/

RUN dotnet restore CommsManager.Web/CommsManager.Web.csproj

# Copy all source code and publish
COPY . .
WORKDIR /src/CommsManager.Web
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CommsManager.Web.dll"]
```

### 5.4 Networking

- All services run on a **user‑defined bridge network** created automatically by Docker Compose.
- Services communicate via **service names** (e.g., `api`, `sqlserver`) which resolve to their internal container IPs.
- Ports are mapped to the host for external access (API on 5000, Web on 5001).

### 5.5 Volume Management

A named volume `sql_data` is used to persist SQL Server data across container restarts. This ensures that database files are not lost when the container is stopped or removed.

### 5.6 Environment Variables

| Variable | Used by | Purpose |
|----------|---------|---------|
| `DB_PASSWORD` | SQL Server, API | Strong password for SA user (required for SQL Server) |
| `ConnectionStrings__DefaultConnection` | API | EF Core connection string (overrides `appsettings.json`) |
| `ApiBaseUrl` | Web | Base URL for the API (Web → API communication) |

All sensitive variables are stored in a `.env` file (excluded from version control).

### 5.7 Healthchecks

- **SQL Server**: Uses `sqlcmd` to verify that the database is ready.
- **API** and **Web**: No healthchecks defined, but they wait for SQL Server via `depends_on`.

### 5.8 Build and Run Commands

```bash
# Build and start all services
docker-compose up -d --build

# Stop and remove containers
docker-compose down

# View logs for a specific service
docker-compose logs -f api

# Rebuild a single service
docker-compose build --no-cache web
```

### 5.9 Development vs. Production

- **Development**: The current configuration is optimized for local development (debug‑friendly, hot reload not used).
- **Production**: For production, you would typically:
  - Use a more secure password
  - Set up HTTPS with certificates
  - Enable healthchecks for API/Web
  - Use a production database (e.g., Azure SQL, AWS RDS)
  - Add a reverse proxy (e.g., Nginx, Traefik) for SSL termination

### 5.10 Future Improvements

- Use **Docker secrets** or **AWS Secrets Manager** for sensitive data
- Add a **reverse proxy** container (e.g., Nginx) to handle SSL and routing
- Implement **CI/CD** with GitHub Actions to build and push images to a registry
- Add **healthcheck endpoints** in API/Web for better orchestration

```

---

## Исправление в README.md

В строке с `└── .env` замените на `└── .env.example` (так как сам `.env` не должен быть в репозитории):

```diff
- └── .env                        # Environment variables template
+ └── .env.example                # Environment variables template
```
