# Project Management Frontend

React TypeScript application for managing projects and tasks.

## Technologies

- React 18
- TypeScript
- React Router
- Axios
- Vite

## Project Structure

```
/src
  /pages          - Page components (Login, Register, Dashboard, ProjectDetails)
  /components     - Reusable components (ProtectedRoute)
  /context        - React context (AuthContext)
  /services       - API service (axios instance)
  /types          - TypeScript type definitions
```

## Running

```bash
npm install
npm run dev
```

Application available at: http://localhost:5173

## Environment Variables

Create a `.env` file:

```
VITE_API_URL=https://localhost:5001
```

## Features

- User authentication (login/register)
- Protected routes
- Project management (create, view, delete)
- Task management (create, update, delete, toggle completion)
- Form validation
- Error handling

## Building

```bash
npm run build
```

Output in `dist/` folder.

## Development

```bash
npm run dev    # Start dev server
npm run build  # Build for production
npm run lint   # Run linter
```
