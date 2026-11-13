# Banking Transaction & Fraud Review API

A portfolio project built with ASP.NET Core 8, C#, Entity Framework Core, and SQL Server. It extends the banking analytics work in the parent repository with an operational API for transaction review.

**Demo scope:** synthetic data and transparent rule-based indicators only. This is not a production banking system or a validated machine-learning fraud model.

## Features
- Browse and filter transactions by account, date range, and review status.
- Create transactions with input validation.
- Evaluate transactions against explainable rules: high amount, rapid activity, and unusual location flag.
- Review queue with analyst decisions and notes.
- Dashboard metrics and transaction summaries.
- Swagger/OpenAPI in Development.

## Technology
- .NET 8 / ASP.NET Core Web API
- C# and Entity Framework Core 8
- SQL Server
- xUnit unit tests for the fraud rule engine

## Run locally
1. Install the .NET 8 SDK and SQL Server (LocalDB, SQL Server Express, or a container).
2. Set ConnectionStrings__BankingDb, for example:
   Server=localhost;Database=BankingFraudDemo;Trusted_Connection=True;TrustServerCertificate=True
3. From this folder run:
   dotnet restore BankingFraudAnalytics.sln
   dotnet run --project src/BankingFraud.Api
4. Open the Swagger URL printed by the app (usually /swagger in Development).

The app creates the demo schema and seeds a small synthetic dataset on first startup. For deployment, replace this demo bootstrap with reviewed EF Core migrations and managed secrets.

## Main endpoints
| Method | Route | Purpose |
|---|---|---|
| GET | /api/transactions | Filter transactions |
| GET | /api/transactions/{id} | Transaction details |
| POST | /api/transactions | Create a transaction |
| GET | /api/reviews | Open review queue |
| POST | /api/reviews/{transactionId} | Record analyst decision |
| GET | /api/dashboard | Summary metrics |

## Example request
{
  "accountId": 1001,
  "amount": 1250.00,
  "currency": "USD",
  "merchant": "Demo Electronics",
  "location": "Phoenix, AZ",
  "occurredAtUtc": "2026-01-15T18:45:00Z"
}

## Project layout
- src/BankingFraud.Api — HTTP endpoints, domain models, EF Core persistence, and rule evaluation.
- tests/BankingFraud.Api.Tests — unit tests for the fraud rules.
- database/analytics-queries.sql — example SQL analytics using aggregation, CTEs, and window functions.
- docs/AI-CODE-REVIEW.md — responsible AI-assisted development and review checklist.

## Security and engineering notes
- Never commit real customer data, credentials, or connection strings.
- Validate inputs and constrain query results.
- Production work requires authentication/authorization, audit logging, rate limiting, migrations, and integration tests.
