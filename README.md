# Credit Simulation API

> **A robust, containerized REST API for financial credit simulation, implementing clean architecture, structured logging, and automated testing.**

## About the Project

This project is a complete modernization and refactoring of a Hackathon challenge. It has been rewritten from scratch using the best practices of the **.NET 8** ecosystem.

The goal is to provide a resilient, observable, and Cloud-Native financial calculation engine (SAC and Price amortization systems), while maintaining compliance with traditional banking audit requirements.

### Key Features

* **Precision Financial Calculations:** Validated algorithms for SAC and Price amortization systems.
* **Hybrid Persistence:**
    * **Oracle Database:** Reads product parameters (simulating Legacy/Mainframe integration).
    * **SQLite:** Transactional storage for simulation history.
* **Aggregated Reports:** Analytical endpoints with data aggregation and enrichment across distinct databases.
* **Server-Side Pagination:** Performance optimization for listing large datasets.
* **Health Checks:** Active dependency monitoring (Liveness/Readiness) for orchestrators (K8s).

---

## Architecture & Patterns

The project follows a Clean Architecture approach focused on maintainability and scalability:

1.  **Hybrid Observability:**
    * **Audit Logs (.txt):** Disk storage with daily rotation for banking compliance.
    * **Telemetry Logs (Seq):** Structured log shipping to a Seq server via Docker for real-time analysis.
    * **Custom Middleware:** Request interception for timing and global error handling.
2.  **Code Quality:**
    * **Unit Testing (xUnit):** Coverage of business logic and services.
    * **Mocking (Moq) & In-Memory DB:** Isolated tests without external infrastructure dependencies.
3.  **DevOps:**
    * **Docker Multi-Stage Build:** Optimized images for production.
    * **Docker Compose:** Full environment orchestration (API + Logs).

---

## Tech Stack

* **Core:** C# .NET 8 (Web API)
* **ORM:** Entity Framework Core
* **Databases:** Oracle (ODP.NET Provider) and SQLite
* **Logging:** Serilog (Sinks for File, Console, and Seq)
* **Documentation:** Swagger / OpenAPI
* **Testing:** xUnit, Moq, FluentAssertions
* **Containerization:** Docker and Docker Compose

---

## How to Run

### Prerequisites

* [Docker](https://www.docker.com/) and Docker Compose installed.
* (Optional) .NET 8 SDK for local execution without Docker.

### Environment Configuration (.env)

For security reasons, credentials are not versioned. Create a `.env` file in the project root following the model below:

```env
# Application Settings
ASPNETCORE_ENVIRONMENT=Production
SEQ_URL=http://seq:5341

# Seq Admin Password 
SEQ_ADMIN_PASSWORD=YourPassword

# Oracle Database Credentials (Example)
ORACLE_USER=User
ORACLE_PASSWORD=Password
ORACLE_HOST=Host
ORACLE_PORT=Port
ORACLE_SID=SID
```

## Running with Docker (Recommended)
The easiest way to start the complete environment (API + Log System) is via Docker Compose. In the terminal, at the project root:
```
docker compose up -d --build
```

This command will:
- Build the application and run Unit Tests (Build fails if tests do not pass).
- Start the API container on port 5050.
- Start the Seq container on port 5341.

Access:
- Swagger (API): http://localhost:5050/swagger
- Seq (Logs): http://localhost:5341 (Login: admin / Password: The one set in .env)
- Health Check: http://localhost:5050/health

## Running Locally (Visual Studio / VS Code)
If you wish to debug the code:
Ensure the .env file is configured.
Restore packages: dotnet restore.
Run the application: dotnet run --project Simulador_de_Credito.
Logs will be generated in the /logs folder at the project root.

## Running Tests
The project includes a unit test suite that validates the calculation logic (SAC/Price) and service flow.
```
dotnet test
```
output Example: <br>
Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 23 ms
ORACLE_PORT=porta
ORACLE_SID=SID
