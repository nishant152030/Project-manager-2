# Project Management API

.NET 8 Web API for project and task management with JWT authentication.

## Technologies

- .NET 8
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- BCrypt for password hashing
- Swagger/OpenAPI

## Project Structure

```
/Controllers    - API endpoints (Auth, Projects, Tasks)
/Models         - Database entities (User, Project, TaskItem)
/DTOs           - Data transfer objects
/Services       - Business logic (AuthService)
/Data           - Database context (AppDbContext)
```

## Running

```bash
dotnet restore
dotnet run
```

API available at: https://localhost:5001

Swagger UI: https://localhost:5001/swagger

## Configuration

Edit `appsettings.json` to change:
- Database connection string
- JWT settings (Key, Issuer, Audience)

## Database

SQLite database is created automatically on first run.

File: `projectmanagement.db`

## Authentication

All endpoints except `/api/auth/register` and `/api/auth/login` require JWT authentication.

Include the token in requests:
```
Authorization: Bearer {your-token}
```

## API Endpoints

See main README.md for full endpoint documentation.
