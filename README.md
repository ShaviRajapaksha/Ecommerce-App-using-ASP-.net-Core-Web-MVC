# E-Commerce MVC Application

A complete ASP.NET Core MVC web application demonstrating database relationships (1-to-1, 1-to-Many, Many-to-Many) with PostgreSQL database, featuring full CRUD operations, responsive UI, and Swagger API documentation.

## Features

### Core Features
-  **Complete CRUD Operations** for Users, Products, Categories, and Orders
-  **Three Types of Database Relationships**:
  - **1-to-1**: User ↔ Profile
  - **1-to-Many**: Category → Products
  - **Many-to-Many**: Order ↔ Product (with junction table)
-  **Responsive Bootstrap 5 UI** - Works on all devices
-  **Swagger/OpenAPI Documentation** - Auto-generated API documentation
-  **PostgreSQL Database** - Production-ready database
-  **Search & Filter** - Search products by name or category
-  **Stock Management** - Automatic stock updates when ordering
-  **Order Status Tracking** - Pending, Processing, Shipped, Delivered, Cancelled

### Additional Features
-  Dashboard with statistics
-  Automatic total calculation
-  Real-time price preview
-  Email validation
-  Mobile-responsive design
-  Clean, modern UI with Font Awesome icons
-  Fast performance with Entity Framework Core

##  Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| ASP.NET Core MVC | 8.0 | Web Framework |
| Entity Framework Core | 6.0+ | ORM for database operations |
| PostgreSQL | 14+ | Database |
| Npgsql | 6.0+ | PostgreSQL provider for EF Core |
| Bootstrap | 5.3 | Frontend framework |
| Swagger/Swashbuckle | 6.5+ | API documentation |
| Font Awesome | 6.4 | Icons |

##  Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK** (6.0 or later) - [Download](https://dotnet.microsoft.com/download)
- **PostgreSQL** (13 or later) - [Download](https://www.postgresql.org/download/)
- **JetBrains Rider** (2023.1 or later) - [Download](https://www.jetbrains.com/rider/download/)
- **Git** (optional) - [Download](https://git-scm.com/)

