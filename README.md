# .NET Project Template

A modern, scalable .NET template featuring **Domain-Driven Design (DDD)**, **Event Sourcing**, **CQRS patterns**, and **.NET Aspire** orchestration.

## Features

- **Domain-Driven Design (DDD)**: Well-structured domain models with aggregates and value objects
- **Event Sourcing**: Full audit trail with event-based aggregate persistence
- **CQRS**: Separated read and write models
- **Event Bus**: In-memory event publishing and subscription
- **Entity Framework Core**: PostgreSQL with EF Core migrations
- **.NET Aspire**: Container orchestration with Redis and PostgreSQL
- **OpenAPI**: Built-in API documentation with Scalar
- **Serilog**: Structured logging
- **Vue.js**: Modern frontend with Vite
- **.NET 10**: Latest C# features and performance improvements

## Quick Start

### Prerequisites

- .NET 10 SDK
- Docker & Docker Compose (for Aspire)
- PowerShell

### 1. Clone the Repository

```bash
git clone https://github.com/pmdevers/dotnet-project-template.git
cd dotnet-project-template
```

### 2. Run with .NET Aspire

The easiest way to run the entire stack:

```bash
dotnet run --project src/AppHost/Template.AppHost.csproj
```

This starts:
- PostgreSQL database
- Redis cache
- ASP.NET Core API
- Vue.js frontend

### 3. Access the Application

- **API**: http://localhost:5000
- **API Docs**: http://localhost:5000/scalar
- **Frontend**: http://localhost:5173

## Architecture

### Domain Layer

- **Aggregates**: `Car`, `Reservation`
- **Value Objects**: `LicensePlate`
- **Events**: `CarRegistered`, `ReservationCreated`, etc.

### Infrastructure Layer

- **Data**: `AppDbContext` with EF Core for PostgreSQL
- **Events**: Event sourcing tables and queries
- **EventBus**: Async event publishing and subscription
- **Migrations**: Database schema versioning

### Features

- **Cars**: Register and query vehicles
- **Reservations**: Create and retrieve reservations (event-sourced)

## Database

### Migrations

Two migrations are included:

1. **InitialCreate**: Creates `Cars` table
2. **EventSourcedAggregate**: Creates `Events` table for event sourcing

Apply migrations automatically on startup via `AppDbContextMigrationService`.

### Connection String

Via `.NET Aspire`:
```csharp
var postgres = builder.AddPostgres("appdb");
```

Automatically injected as `ConnectionStrings:appdb`

Manual setup:
```csharp
"ConnectionStrings": {
  "appdb": "Host=localhost;Database=streamsharp;Username=postgres;Password=postgres"
}
```

## Key Patterns

### Event Sourcing Example

```csharp
// Create a reservation (event-sourced aggregate)
var reservation = Reservation.Create(customerId, carId, startDate, endDate);
var repo = serviceProvider.GetRequiredService<IRepository<Reservation, ReservationId>>();
repo.Add(reservation);
await unitOfWork.SaveChangesAsync();

// Load from history
var loaded = await repo.TryFindAsync(reservation.Id);
```

### CQRS Pattern

```csharp
// Command: create via endpoint
app.MapPost("/reservations", CreateReservation.Handle);

// Query: read via ICarQueries or EventQueries
var cars = await carQueries.GetAllAsync();
var reservation = await dbContext.GetAggregate<Reservation>(id);
```

### Event Bus

```csharp
// Subscribe to events
services.AddEventBus();

// Publish automatically on domain events
await unitOfWork.SaveChangesAsync(); // Triggers event dispatch
```

## Common Tasks

### Add a New Aggregate

1. Create entity in `Domain/Entities`
2. Create configuration in `Infrastructure/Data/Config`
3. Add `DbSet<T>` to `AppDbContext`
4. Set Environment variable for connection string (if needed): `$env:POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=postgres'` 
4. Create migration: `dotnet ef migrations add "{YourMigrationName}" --project src\Api\Template.Api.csproj --output-dir Infrastructure/Data/Migrations`

### Add an Endpoint

1. Create feature in `Features/`
2. Implement handler with dependency injection
3. Register in `ApiEndpoints.cs`

### Subscribe to Domain Events

```csharp
// In EventBusExtensions.cs
bus.Subscribe<CarRegistered>(async @event =>
{
    logger.LogInformation("Car registered: {@Event}", @event);
});
```

## NuGet Packages

Core dependencies:
- `Microsoft.EntityFrameworkCore` (10.0.0)
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Microsoft.AspNetCore.OpenApi`
- `Serilog` & related sinks

## Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/Api/Template.Api.csproj
```

## API Documentation

After running the app, visit:
- **Scalar UI**: http://localhost:5000/scalar
- **OpenAPI JSON**: http://localhost:5000/openapi/v1.json

## Troubleshooting

### Database connection fails
- Ensure PostgreSQL is running (via Aspire or Docker)
- Check `POSTGRES_CONNECTION_STRING` environment variable

### Migrations don't apply
- Delete `AppDbContextMigrationService` output and retry
- Verify EF Core tools: `dotnet ef --version`

### Aspire not found
- Update .NET: `dotnet --version` should be 10.0.0+
- Reinstall: `dotnet tool update -g dotnet-aspire-latest`

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Commit changes
4. Push and open a pull request

## License

This project is open source and available under the MIT License.

## Resources

- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)

---

**Created by**: [@pmdevers](https://github.com/pmdevers)

**Repository**: https://github.com/pmdevers/dotnet-project-template
