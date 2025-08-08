# Microservices Architecture Project

This project implements a modern microservices architecture using .NET 9.0, demonstrating best practices in distributed systems design and implementation.

## Project Structure

The solution is organized into the following main components:

### Gateway
- **ApiGateway** - Reverse proxy implementation using YARP, handling routing and authentication of requests to internal services.
  - JWT authentication integration
  - Swagger/OpenAPI documentation
  - Request routing and load balancing

### Services

#### Authentication Service
- **AuthService** - Handles user authentication and authorization
  - JWT token generation and validation
  - User management
  - SQLite database integration
  - gRPC service implementation for inter-service communication
  - BCrypt for password hashing

#### Review Service
- Clean Architecture implementation with separate projects:
  - **ReviewService.Domain** - Contains business entities and interfaces
  - **ReviewService.Application** - Business logic and use cases
  - **ReviewService.Infrastructure** - Data access and external service implementations
  - **ReviewService.Web** - API controllers and configuration
  - Features:
    - Protected endpoints with JWT authentication
    - SQLite database for data persistence
    - gRPC client for auth service communication

### Frontend
- **presentation** - Modern web application built with:
  - Svelte framework
  - Vite build tool
  - Custom API client
  - User authentication integration
  - Star rating component

## Technologies

- **.NET 9.0** - Latest version of the .NET platform
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database for each service
- **JWT** - Token-based authentication
- **gRPC** - High-performance RPC framework for inter-service communication
- **Swagger/OpenAPI** - API documentation and testing
- **YARP** - Reverse proxy for API Gateway
- **Svelte** - Frontend framework
- **Vite** - Next generation frontend tooling

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Node.js and npm (for frontend)
- IDE (Visual Studio, Rider, or VS Code)

### Setup

1. Clone the repository
2. Start the services:
   ```bash
   # Start Auth Service
   cd Services/AuthService
   dotnet run

   # Start Review Service
   cd Services/ReviewService/ReviewService.Web
   dotnet run

   # Start API Gateway
   cd Gateway/ApiGateway
   dotnet run
   ```

3. Start the frontend:
   ```bash
   cd presentation
   npm install
   npm run dev
   ```

## Architecture Overview

The project follows a microservices architecture pattern with:

- Independent services with their own databases
- API Gateway for centralized routing and authentication
- Service-to-service communication via gRPC
- Clean Architecture principles in service implementation
- Token-based authentication
- Modern frontend with component-based architecture

## Security

- JWT-based authentication
- Secure password hashing with BCrypt
- Protected API endpoints
- HTTPS communication

## Additional Features

- Star rating system
- User reviews management
- Modern UI components
- Real-time updates
- Responsive design