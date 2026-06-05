# Solution Folder Guide

This guide defines what belongs in each folder represented by the solution and
its projects.

## Top-Level Solution Folders

### src/

Production code lives here. Keep reusable and deployable components under
project-specific folders.

Contains:

- `AppHost/`: Aspire orchestration and deployment composition.
- `Api/`: ASP.NET Core application with domain and infrastructure.
- `Generators/`: Roslyn source generators and analyzers.
- `ServiceDefaults/`: Shared service configuration extensions.
- `WebUi/`: Frontend app and embedded static assets.

Does not contain:

- Unit tests.
- Temporary scripts or scratch files.
- Build artifacts (bin/obj output only).

### test/

Automated tests live here.

Contains:

- `Template.Tests/`: Domain and feature tests for the template API.

Does not contain:

- Production implementation code.
- Manual test notes or ad-hoc run output.

## Project Folder Responsibilities

### src/AppHost/

Use this project for distributed application orchestration.

Put here:

- Resource wiring for API, Redis, PostgreSQL, and related infrastructure.
- Deployment and image publishing configuration used by Aspire.
- Environment-specific orchestration settings.

Avoid here:

- Business logic.
- HTTP endpoint handlers.

### src/Api/

Core backend application code.

Put here:

- API endpoints and request handlers.
- Domain entities, value objects, and domain abstractions.
- Infrastructure integrations (data access, event bus, persistence).
- Runtime configuration and middleware registration.

Avoid here:

- Frontend-only assets.
- Test-only helper code.

### src/Api/Configuration/

Application setup and runtime wiring.

Put here:

- DI registrations.
- options classes and binding.
- Middleware and host configuration extensions.

### src/Api/Domain/

Business rules and domain model.

Put here:

- Entities and aggregates.
- Value objects and IDs.
- Domain events and domain interfaces.

Keep this layer independent from infrastructure details.

### src/Api/Features/

Vertical slices for use cases.

Put here:

- Endpoint mapping for each feature area.
- Command/query handlers for API operations.

Avoid direct coupling to infrastructure implementation types when domain
abstractions can be used.

### src/Api/Infrastructure/

Technical implementation details.

Put here:

- EF Core DbContext, migrations, and query implementations.
- Event bus implementation and serialization.
- Persistence-related services.

### src/Generators/

Compile-time analyzers and source generation.

Put here:

- Analyzer diagnostics and rules.
- Code-generation templates and generator implementations.
- Release notes for analyzer versions.

### src/ServiceDefaults/

Cross-cutting service behavior shared by applications.

Put here:

- Logging, telemetry, resilience, and discovery defaults.
- Extension methods consumed by host projects.

### src/WebUi/

Frontend source and build packaging for embedding in .NET artifacts.

Put here:

- Vite and TypeScript configuration.
- Vue components and static frontend assets.
- Build output packaging logic used by the .NET project.

Avoid here:

- Backend domain logic.

### test/Template.Tests/

Test project for template behavior.

Put here:

- Unit and feature tests for domain and API handlers.
- Test doubles and helper fixtures used by tests.

Avoid here:

- Application startup or deployment code.

## Non-Solution Support Folders

These folders support development but are not primary solution code folders.

- `.github/`: CI/CD and automation workflows.
- `docs/`: Documentation source files for DocFX and Pages.
- `assets/`: Shared documentation assets.
- `artifacts/`: Generated build outputs.
