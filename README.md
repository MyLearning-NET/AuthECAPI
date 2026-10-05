# AuthECAPI

ASP.NET Core Web API (.NET 10) for user registration and login. It uses ASP.NET Core Identity with Entity Framework Core and SQL Server for user storage, and issues JWT access tokens on sign-in.

## Tech Stack

- .NET 10 / ASP.NET Core Web API (minimal API endpoints plus MVC controllers)
- ASP.NET Core Identity with a custom `AppUser` (adds `FullName`)
- Entity Framework Core 10 (SQL Server provider, Code First migrations)
- JWT Bearer authentication (HS256)
- Swagger UI (Swashbuckle) and built-in OpenAPI document

## Project Structure

```
AuthECAPI/
├── AuthECAPI.slnx
└── AuthECAPI/
    ├── Controllers/
    │   └── IdentityUserEndpoints.cs
    ├── Extensions/
    │   ├── AppConfigExtensions.cs
    │   ├── EFCoreExtensions.cs
    │   ├── IdentityExtensions.cs
    │   └── SwaggerExtensions.cs
    ├── Models/
    │   ├── AppDbContext.cs
    │   ├── AppSettings.cs
    │   └── AppUser.cs
    ├── Migrations/
    ├── appsettings.json
    └── Program.cs
```

Service and middleware setup is split into extension methods so that `Program.cs` stays short.

## API Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/signup` | Register a user |
| POST | `/api/signin` | Validate credentials and return a JWT |

The built-in ASP.NET Core Identity API endpoints are also mapped under `/api` (for example `/api/register`, `/api/login`, `/api/refresh`).

Request body for `/api/signup`:

```json
{
  "email": "jane@example.com",
  "password": "secret1",
  "fullName": "Jane Doe"
}
```

Request body for `/api/signin`:

```json
{
  "email": "jane@example.com",
  "password": "secret1"
}
```

Response from `/api/signin`:

```json
{
  "token": "<jwt>"
}
```

Tokens carry a `UserID` claim and expire after 10 days.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installed with Visual Studio on Windows) or any SQL Server instance
- EF Core CLI tool

```bash
dotnet tool install --global dotnet-ef
```

### Setup

```bash
git clone https://github.com/<your-username>/AuthECAPI.git
cd AuthECAPI/AuthECAPI
dotnet restore
dotnet ef database update
dotnet run
```

The API starts at `http://localhost:5265`. Swagger UI is available at `http://localhost:5265/swagger`.

### Configuration

`appsettings.json` contains a placeholder `JWTSecret` and a LocalDB connection string named `DevDB`. Do not put a real signing key in this file. Override both values locally:

```bash
dotnet user-secrets init
dotnet user-secrets set "AppSettings:JWTSecret" "<random-string-of-at-least-32-characters>"
dotnet user-secrets set "ConnectionStrings:DevDB" "Server=localhost,1433;Database=AuthECDB;User Id=sa;Password=<password>;TrustServerCertificate=True"
```

## Known Limitations

- No endpoint currently requires authorization, so the JWT middleware is configured but not exercised.
- Token issuer and audience are not set or configured in the validation parameters.
- `UseCors()` is called without a registered CORS policy.
- Password rules are relaxed (no digit, uppercase or lowercase requirement) for development.
- No request validation on the signup and signin models.
