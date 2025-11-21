# Stadium Analytics

## Overview
Stadium Analytics is a .NET 8 Web API that aggregates per-minute sensor events from stadium gates and returns grouped analytics (by gate and enter/leave type). Useful for monitoring people flow and optimizing staffing.

## Quick start (Windows / PowerShell)

Prerequisites
- .NET 8 SDK
- (optional) Docker & docker-compose
- (optional) dotnet-ef: `dotnet tool install --global dotnet-ef`

Run from source
```powershell
cd '.\StadiumAnalytics\StadiumAnalytics\src\StadiumAnalytics.Api'
dotnet restore
dotnet build
dotnet run
```

Check console output for listening URLs (e.g. http://localhost:5000 and https://localhost:5001).

Database
- SQLite file created at: src/StadiumAnalytics.Api\stadium.db
- Quick (no-migrations): Program.cs calls `db.Database.EnsureCreated()` on startup.
- Recommended: use EF migrations to track schema:

```powershell
cd src\StadiumAnalytics.Api
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate --project . --startup-project .
dotnet ef database update --project . --startup-project .
```

## API (current)
- GET /api/sensors
  - Optional query parameters: gate, type, start, end
  - Example response:
    ```json
    [
      { "gate":"Gate A", "type":"enter", "numberOfPeople":100 }
    ]
    ```
- POST /api/sensors/events
  - Accepts a single sensor event JSON to persist (useful for tests).
  - Example payload:
    ```json
    {
      "gate":"Gate A",
      "timestamp":"2025-11-21T12:00:00Z",
      "numberOfPeople":5,
      "type":"enter"
    }
    ```

## Sample requests

curl (Linux / macOS / Windows with curl)
```bash
# GET all aggregated results (no filters)
curl "http://localhost:5000/api/sensors"

# GET with filters (gate, type, optional ISO8601 start/end)
curl "http://localhost:5000/api/sensors?gate=Gate%20A&type=enter&start=2025-11-21T00:00:00Z&end=2025-11-21T23:59:59Z"

# POST a single sensor event (bash)
curl -X POST "http://localhost:5000/api/sensors/events" \
  -H "Content-Type: application/json" \
  -d '{"gate":"Gate A","timestamp":"2025-11-21T12:00:00Z","numberOfPeople":5,"type":"enter"}'
```

## Simulating events

Two ways to generate test events so the API receives and persists sensor data:

1) Built-in EventSimulator (recommended)
- Ensure the simulator is registered in Program.cs:
  ```csharp
  builder.Services.AddHostedService<EventSimulator>();
  ```
- Run the API (simulator runs in-process)

## Testing
- Test project: tests\StadiumAnalytics.Tests
- Ensure test project has test packages and a reference to the API project:
```powershell
cd tests\StadiumAnalytics.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package NUnit
dotnet add package NUnit3TestAdapter
dotnet add package Moq
dotnet add reference '..\..\src\StadiumAnalytics.Api\StadiumAnalytics.Api.csproj'
dotnet test
```
If the runner reports "No tests found", verify the test csproj targets net8.0 and includes Microsoft.NET.Test.Sdk and NUnit3TestAdapter.

## Docker
Build and run (example)
TODO.

## Troubleshooting
- "no such table: SensorEvents" — ensure DB is created (EnsureCreated or apply EF migrations).
- If POST succeeds but GET shows no data, inspect stadium.db with sqlite3:
```powershell
cd src\StadiumAnalytics.Api
sqlite3 stadium.db ".schema"
sqlite3 stadium.db "select * from SensorEvents limit 20;"
```

## Notes
- An event simulator may run as a hosted background service (optional) to generate sample events.
- Seed data may be added on first run for quick verification.

## License
MIT License.
