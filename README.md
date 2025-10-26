# Project Management System

A full-stack project and task management application with user authentication, built with .NET 8 and React. This repository contains a .NET Web API backend and a React + TypeScript frontend (Vite).

Badges: [![.NET](https://img.shields.io/badge/.NET-8-brightgreen)](https://dotnet.microsoft.com/) [![React](https://img.shields.io/badge/React-18-blue)](https://reactjs.org/) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Table of contents

- Tech Stack
- Prerequisites
- Quick Start
- Configuration
- Features
- API Endpoints
- Project Structure
- Development
- Database
- Troubleshooting
- Contributing
- License

## Tech Stack

**Backend:**
- .NET 8 Web API
- Entity Framework Core
- SQLite Database
- JWT Authentication
- BCrypt Password Hashing

**Frontend:**
- React 18 with TypeScript
- React Router for navigation
- Axios for API calls
- Vite for build tooling

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- npm (comes with Node.js)

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/nishant152030/Project-manager-2.git
cd Project-manager-2
```

### 2. Backend Setup

```bash
cd backend
dotnet restore
# optional: dotnet ef database update (if using migrations)
dotnet run
```

The API will be available at `https://localhost:5001` by default.

Swagger documentation: `https://localhost:5001/swagger`

Tip: During development you can use hot reload:

```bash
cd backend
dotnet watch run
```

### 3. Frontend Setup

Open a new terminal:

```bash
cd frontend
npm install
npm run dev
```

The application will be available at `http://localhost:5173`.

## Configuration

### Backend Configuration

Edit `backend/appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=projectmanagement.db"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters",
    "Issuer": "ProjectManagementAPI",
    "Audience": "ProjectManagementClient"
  }
}
```

Make sure to set a strong JWT key in production and keep it secret (use environment variables or a secrets manager).

### Frontend Configuration

The frontend is pre-configured with `.env`:

```
VITE_API_URL=https://localhost:5001
```

Update this if your backend runs on a different port or host.

## Features

- User registration and authentication
- JWT-based authorization
- Create and manage projects
- Create, update, and delete tasks
- Mark tasks as complete/incomplete
- Protected routes and API endpoints

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Projects (Requires Authentication)
- `GET /api/projects` - Get all user projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### Tasks (Requires Authentication)
- `GET /api/tasks/project/{projectId}` - Get all tasks for a project
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task
- `PATCH /api/tasks/{id}/toggle` - Toggle task completion

See `API_TESTING.md` (if present) for detailed API testing examples.

## Project Structure

```
.
├── backend/
│   ├── Controllers/      # API endpoints
│   ├── Models/           # Database entities
│   ├── DTOs/             # Data transfer objects
│   ├── Services/         # Business logic
│   ├── Data/             # Database context
│   └── Program.cs        # Application entry point
│
└── frontend/
    └── src/
        ├── pages/        # Page components
        ├── components/   # Reusable components
        ├── context/      # React context
        ├── services/     # API service
        └── types/        # TypeScript types
```

For more details, see `backend/README.md` and `frontend/README.md`.

## Development

### Backend

```bash
cd backend
dotnet watch run    # Run with hot reload
```

### Frontend

```bash
cd frontend
npm run dev         # Development server
npm run build       # Production build
npm run lint        # Run ESLint
```

## Database

The SQLite database is created automatically on first run at `backend/projectmanagement.db`.

To reset the database, delete the `.db` file and restart the backend. If your project uses EF migrations, run `dotnet ef database update` to apply them.

## Troubleshooting

- CORS Issues: Ensure the frontend URL is listed in the CORS policy in `backend/Program.cs`.
- SSL Certificate Warnings: The backend uses HTTPS in development. Accept the self-signed certificate or configure your browser to trust it.
- Port Conflicts: If ports 5001 or 5173 are in use, update the configuration files accordingly.

## Contributing

Contributions are welcome — please open an issue or submit a pull request. In issues/PRs, include a clear description of the change and, where applicable, screenshots or steps to reproduce.

## License

MIT
