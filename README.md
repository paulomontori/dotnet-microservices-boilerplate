# Dotnet Microservices Boilerplate

This repository provides a minimal starting point for building microservice-based applications with **.NET 9**. The structure is intentionally light and serves as a foundation to grow custom services.

## Repository Layout

- `src/` – Source code for services
  - `OrderService/`
    - `API/` – HTTP controllers
    - `Application/` – Application-level logic (currently empty)
    - `Domain/` – Domain entities and interfaces
    - `Infrastructure/` – Data access and other infrastructure concerns
- `tests/` – Unit test projects
- `docker-compose.yml` – Container orchestration placeholder

## Getting Started

1. Install the .NET SDK (version 9.0 or later).
2. Restore dependencies and build:
   ```bash
   dotnet build
   ```
3. Run tests (if any):
   ```bash
   dotnet test
   ```

This boilerplate does not include specific implementations, allowing you to tailor services, infrastructure and tests to your needs.
