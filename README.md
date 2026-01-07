# Multitenant UI Application – ASP.NET Core 10

This repository contains a **Multitenant UI-only application** built with **ASP.NET Core 10**.  
The project provides user-facing pages for CRUD operations and communicates with backend APIs using **Refit**.

Authentication is currently implemented using **Identity Server**, and the solution is designed to support **JWT-based authentication** for API access in the future.

API URL: [DotNetCoreWithIdentityServer4](https://github.com/MAsimShah/DotNetCoreWithIdentityServer4)
---

## Project Purpose

- Acts as a **UI / Presentation Layer**
- Handles user interaction and UI-based CRUD operations
- Connects to backend APIs via Refit
- Prepares the foundation for JWT-secured API communication
- Supports **multitenant architecture**

---

## Tech Stack

- ASP.NET Core 10
- Razor Pages / MVC
- Identity Server
- Refit (HTTP API client)
- Dependency Injection
- Async/Await


## Authentication

### Current
- Identity Server is used for authentication
- Manages login, logout, and user identity within the UI

### Planned
- JWT token generation
- JWT-based authorization for API calls
- Secure API-to-UI communication

---

## Multitenancy

- Tenant-aware UI design
- Ready for tenant resolution via:
  - Headers
  - Claims
  - Subdomains (future support)

---

## API Communication

- Uses **Refit** for consuming APIs
- Strongly typed API interfaces
- Clean and maintainable HTTP calls
- JWT token support planned for authorization headers

---

## Features

- UI pages for CRUD operations
- Authentication-protected pages
- Clean separation of concerns
- Scalable and maintainable structure
- Ready for enterprise-level extension

---

## Getting Started

### Prerequisites
- .NET SDK 10
- Visual Studio 2022 or VS Code

### Run the Application
```bash
dotnet restore
dotnet build
dotnet run

