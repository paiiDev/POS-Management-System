# POS.App - Point of Sale System

[![.NET](https://img.shields.io/badge/.NET-8.0-purple?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Overview

**POS.App** is a full-stack Point of Sale (POS) web application built with modern .NET technologies. Designed for small businesses, this system streamlines sales operations, inventory management, and user administration through an intuitive AdminLTE-based interface.

## Key Features

- **Authentication & Authorization**: Secure login system with role-based access control (RBAC) using Cookie Authentication
- **Product Management**: Create, edit, delete, and view products with categories
- **Category Management**: Organize products into categories
- **Sales Operations**: Process sales transactions, generate invoices, and void sales
- **User Administration**: Manage system users with different roles
- **Dashboard**: Overview of business operations
- **Invoice Generation**: Generate detailed invoices for sales transactions

## Tech Stack

### Backend

- **.NET 8.0** - Modern .NET framework
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core 8.0** - ORM for database access
- **SQL Server** - Relational database
- **BCrypt.Net-Next** - Password hashing library

### Frontend

- **Razor Views** - Server-side rendering
- **AdminLTE** - Admin dashboard template
- **Bootstrap** - CSS framework
- **Font Awesome** - Icon library
- **Chart.js** - Data visualization

### Architecture

- **Clean Architecture / N-Layered Architecture**
  - **POS.App** (Presentation Layer)
  - **POS.Domain** (Business Logic Layer)
  - **POS.Database** (Data Access Layer)
  - **POS.Shared** (Shared Utilities & Common Components)

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     POS.App (Presentation)                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │ Controllers  │  │    Views     │  │  wwwroot     │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   POS.Domain (Business Logic)                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │  Services    │  │  Helpers     │  │ Interfaces   │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  POS.Database (Data Access)                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │  DbContext   │  │ Repositories │  │ Interfaces   │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   POS.Shared (Common)                        │
│  ┌──────────────┐  ┌──────────────┐                          │
│  │   Common     │  │  Utilities   │                          │
│  └──────────────┘  └──────────────┘                          │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    SQL Server Database                      │
└─────────────────────────────────────────────────────────────┘
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/POS.App.git
   cd POS.App
   ```

2. Configure the database connection:
   - Open `appsettings.Development.json` in the `POS.App` project
   - Update the `DBConnection` connection string to point to your SQL Server instance

3. Build the solution:

   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   cd POS.App
   dotnet run
   ```

### Default Admin Credentials

The application automatically seeds a default admin user:

- **Username**: `admin`
- **Password**: `Admin123`

## Project Structure

```
POS.App/
├── POS.App/                 # Presentation layer (MVC Controllers, Views, wwwroot)
│   ├── Controllers/
│   ├── Views/
│   └── wwwroot/
├── POS.Domain/              # Business logic layer (Services, Helpers)
├── POS.Database/            # Data access layer (DbContext, Repositories)
├── POS.Shared/              # Shared utilities and common components
└── POS.App.sln              # Solution file
```

## Key Design Patterns Implemented

- **Repository Pattern** - For data access abstraction
- **Service Layer Pattern** - For business logic separation
- **Dependency Injection** - Built-in ASP.NET Core DI container
- **Separation of Concerns** - Through N-layered architecture

## Future Enhancements

- [ ] REST API for third-party integrations
- [ ] Inventory stock alerts and notifications
- [ ] Advanced reporting and analytics
- [ ] Customer management system
- [ ] Multi-store/branch support
- [ ] Payment gateway integration

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Built with ❤️ using .NET 8.0**
