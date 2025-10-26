# API Testing Guide

## Using cURL

### Register a User

```bash
curl -X POST http://localhost:5078/api/auth/register \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"testuser\",\"password\":\"password123\"}"
```

### Login

```bash
curl -X POST http://localhost:5078/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"testuser\",\"password\":\"password123\"}"
```

Save the token from the response.

### Create a Project

```bash
curl -X POST http://localhost:5078/api/projects \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d "{\"title\":\"My First Project\",\"description\":\"Project description\"}"
```

### Get All Projects

```bash
curl -X GET http://localhost:5078/api/projects \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Get Project Details

```bash
curl -X GET http://localhost:5078/api/projects/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Create a Task

```bash
curl -X POST http://localhost:5078/api/projects/1/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d "{\"title\":\"My First Task\",\"dueDate\":\"2025-12-31\"}"
```

### Get Tasks for a Project

```bash
curl -X GET http://localhost:5078/api/projects/1/tasks \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Update a Task

```bash
curl -X PUT http://localhost:5078/api/tasks/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d "{\"isCompleted\":true}"
```

### Delete a Task

```bash
curl -X DELETE http://localhost:5078/api/tasks/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Delete a Project

```bash
curl -X DELETE http://localhost:5078/api/projects/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Using PowerShell

### Register a User

```powershell
$body = @{
    username = "testuser"
    password = "password123"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5078/api/auth/register" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"
```

### Login and Save Token

```powershell
$body = @{
    username = "testuser"
    password = "password123"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:5078/api/auth/login" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"

$token = $response.token
```

### Create a Project

```powershell
$headers = @{
    Authorization = "Bearer $token"
}

$body = @{
    title = "My First Project"
    description = "Project description"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5078/api/projects" `
  -Method Post `
  -Headers $headers `
  -Body $body `
  -ContentType "application/json"
```

## Using Swagger UI

1. Navigate to http://localhost:5078/swagger
2. Click the "Authorize" button at the top
3. Register or login using the auth endpoints
4. Copy the token from the response
5. Enter "Bearer {token}" in the authorization dialog
6. Click "Authorize"
7. Now you can test all endpoints directly from the UI

## Response Codes

- 200 OK - Request successful
- 201 Created - Resource created successfully
- 204 No Content - Delete successful
- 400 Bad Request - Validation error
- 401 Unauthorized - Invalid or missing token
- 404 Not Found - Resource not found

## Notes

- Replace `YOUR_TOKEN_HERE` with the actual JWT token from login/register
- Project and task IDs are auto-incremented integers
