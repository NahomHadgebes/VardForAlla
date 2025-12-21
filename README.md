# VårdFörAlla - Medical Routine Management System

A professional system for managing medical routines and translations, built for Swedish healthcare with focus on multilingualism and user experience.

> **Note**: The application interface is in Swedish (Svenska), designed for Swedish healthcare professionals.

## 📋 Project Description

VårdFörAlla is a web application that enables healthcare professionals to:
- Create, edit, and manage medical routines with step-by-step instructions
- Translate routines into multiple languages (English, Arabic, Somali)
- Organize routines with categories and tags
- Ensure correct medical terminology alongside patient-friendly language
- Manage both system template routines (for administrators) and personal routines

### Key Technologies
- **Backend**: ASP.NET Core 8.0 Web API
- **Frontend**: React 19 with TypeScript and Tailwind CSS
- **Database**: SQL Server with Entity Framework Core 9.0
- **Authentication**: JWT-based authentication with role-based access control
- **State Management**: TanStack Query (React Query)
- **Routing**: React Router v7 with HashRouter

## 🏗️ Architecture Overview

The project follows Clean Architecture principles with clear separation of concerns:

```
VardForAlla/
├── VardForAlla.Domain/          # Domain models and business entities
│   ├── Entities/                # Domain entities (User, Routine, RoutineStep, etc.)
│   ├── Builders/                # Builder pattern for complex object creation
│   └── Factories/               # Factory pattern for entity creation
│
├── VardForAlla.Application/     # Application logic and use cases
│   ├── Interfaces/              # Service and repository interfaces
│   ├── Services/                # Business logic implementation
│   └── Factories/               # Application-level factories
│
├── VardForAlla.Infrastructure/  # External concerns and data access
│   ├── Data/                    # EF Core DbContext and migrations
│   ├── Repositories/            # Repository implementations
│   ├── Services/                # Infrastructure services (hashing, tokens)
│   └── Seeding/                 # Database seeding
│
├── VardForAlla.Api/             # Web API layer
│   ├── Controllers/             # API endpoints
│   ├── Dtos/                    # Data Transfer Objects
│   ├── DtoBuilder/              # DTO mapping logic
│   └── Middleware/              # Global exception handling
│
├── VårdForAlla.Frontend/        # React SPA
│   ├── components/              # Reusable UI components
│   ├── views/                   # Page components
│   ├── context/                 # React context (Auth, Toast)
│   ├── hooks/                   # Custom React hooks
│   ├── services/                # API client configuration
│   └── types.ts                 # TypeScript type definitions
│
└── VardForAlla.Tests/           # Unit tests
    └── Services/                # Service layer tests
```

### Design Patterns
- **Repository Pattern**: Abstraction over data access
- **Factory Pattern**: For complex entity creation (RoutineFactory)
- **Builder Pattern**: For fluent object construction (RoutineBuilder)
- **Dependency Injection**: Throughout all layers
- **DTO Pattern**: Clean separation between API and domain models

### Database Schema
Key entities and relationships:
- **Users** → UserRoles ← **Roles** (Many-to-Many)
- **Users** → **Routines** (One-to-Many, nullable FK)
- **Routines** → **RoutineSteps** (One-to-Many, cascade delete)
- **Routines** ↔ **Tags** (Many-to-Many)
- **RoutineSteps** → **StepTranslations** (One-to-Many, cascade delete)
- **Languages** → **StepTranslations** (One-to-Many, restrict delete)

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+ and npm
- SQL Server 2019+ (or SQL Server Express)

### 1. Database Setup

**Configure connection string** in `VardForAlla.api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VardForAllaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> **Note**: Replace `SQLEXPRESS` with your SQL Server instance name (e.g., `SQLEXPRESS04`, `MSSQLSERVER`, etc.)

**Run migrations** from the solution root:
```bash
cd VardForAlla.api
dotnet ef database update
```

This will:
- Create the database schema
- Seed default roles (Admin, User)
- Create an admin account:
  - Email: `admin@vardforalla.se`
  - Password: `Admin123!`
- Seed supported languages (Svenska, Engelska, Arabiska, Somaliska, Polska, Finska)

### 2. Backend Setup

From the solution root:

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API (from VardForAlla.api directory)
cd VardForAlla.api
dotnet run
```

The API will start at:
- **HTTPS**: `https://localhost:7144`
- **HTTP**: `http://localhost:5066`
- **Swagger UI**: `https://localhost:7144/swagger`

### 3. Frontend Setup

From the frontend directory:

```bash
cd VårdForAlla.Frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will start at:
- **Development**: `http://localhost:3000`

### 4. Login Credentials

**Administrator Account:**
- Email: `admin@vardforalla.se`
- Password: `Admin123!`

To create additional users, register through the frontend or use the `/api/auth/register` endpoint.

## 📡 API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/login` | Login with email and password | No |
| POST | `/register` | Register new user account | No |
| POST | `/request-password-reset` | Request password reset token | No |
| POST | `/reset-password` | Reset password with token | No |
| GET | `/me` | Get current user profile | Yes |

### Routines (`/api/routine`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/` | Get all routines (with filtering) | Yes |
| GET | `/{id}` | Get routine by ID | Yes |
| POST | `/` | Create new routine | Yes |
| PUT | `/{id}` | Update routine | Yes |
| DELETE | `/{id}` | Delete routine (soft delete) | Yes |

**Query Parameters for GET /:**
- `search` - Filter by title or description
- `category` - Filter by category

### Routine Steps (`/api/routines/{routineId}/steps`, `/api/steps/{id}`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/routines/{routineId}/steps` | Get all steps for a routine | Yes |
| POST | `/routines/{routineId}/steps` | Add step to routine | Yes |
| PUT | `/steps/{id}` | Update a step | Yes |
| DELETE | `/steps/{id}` | Delete a step | Yes |

### Step Translations (`/api/steps/{stepId}/translations`, `/api/translations/{id}`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/steps/{stepId}/translations` | Get translations for a step | Yes |
| POST | `/steps/{stepId}/translations` | Add translation to step | Yes |
| PUT | `/translations/{id}` | Update translation | Yes |
| DELETE | `/translations/{id}` | Delete translation | Yes |

### Languages (`/api/language`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/` | Get all languages | Yes |
| POST | `/` | Create new language | Yes (Admin) |

### Tags (`/api/tag`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/` | Get all tags | Yes |
| GET | `/{id}` | Get tag by ID | Yes |
| POST | `/` | Create new tag | Yes |
| PUT | `/{id}` | Update tag | Yes |
| DELETE | `/{id}` | Delete tag | Yes |

## 🔐 Authentication & Authorization

### Roles
- **Admin**: Full system access, can manage templates and all routines
- **User**: Can view all routines and create/edit personal routines

### JWT Token
- Token expires after 7 days
- Include in requests via `Authorization: Bearer {token}` header
- Frontend automatically handles token storage and injection

## 🧪 Testing

Run unit tests:

```bash
# From solution root
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

Test coverage includes:
- Service layer unit tests (AuthService, RoutineService, LanguageService, etc.)
- Repository integration tests
- Password hashing verification
- Business logic validation

## 🐛 Known Issues & Limitations

### Current Known Bugs
1. **None reported** - The system is currently stable in testing

### Limitations
- **Browser Storage**: Frontend uses localStorage for token persistence (not sessionStorage due to Claude.ai artifact limitations in documentation example)
- **Language Support**: Currently supports Swedish, English, Arabic, Somali, Polish, and Finnish. Additional languages can be added via the API.
- **File Uploads**: Not implemented - routines are text-based only
- **Audit Logging**: Basic logging implemented but no comprehensive audit trail UI
- **Email Notifications**: Password reset generates token but email sending is not implemented
- **Offline Support**: Requires active internet connection

### Future Enhancements
- Advanced search with full-text indexing
- Export routines to PDF
- Image support for routine steps
- Activity feed for routine changes
- Routine versioning and history
- Mobile-optimized responsive views (currently desktop-focused)

## 📝 Development Notes

### Code Conventions
- C# follows standard .NET naming conventions
- TypeScript uses camelCase for variables/functions, PascalCase for components
- All database entities use PascalCase
- API routes use lowercase with hyphens

### Database Migrations
To create a new migration:
```bash
cd VardForAlla.api
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Frontend Build
To build for production:
```bash
cd VårdForAlla.Frontend
npm run build
```

## 📄 License

This project is developed as part of a .NET educational program.

## 👥 Authors

Developed for Swedish healthcare environments with consideration for multilingual patient communication.

---

**Last Updated**: December 2025  
**Version**: 1.0.0
