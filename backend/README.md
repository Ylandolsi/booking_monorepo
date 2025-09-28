# Booking Backend

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Modules](#modules)
  - [API Module](#api-module)
  - [Common Module](#common-module)
  - [Users Module](#users-module)
  - [Catalog Module](#catalog-module)
- [Getting Started](#getting-started)
- [Development](#development)
- [Testing](#testing)

## Overview

This backend application implements a booking/marketplace platform with the following core capabilities:

- User management and authentication (JWT, Google OAuth)
- Product/service catalog management
- Store and order management
- Payment processing and payouts
- Real-time notifications
- File uploads and image processing
- Background job processing

## Project Structure

```
backend/
├── Booking/                              # Main solution folder
│   ├── Booking.sln                       # Solution file
│   ├── .dockerignore                     # Docker ignore patterns
│   ├── .gitignore                        # Git ignore patterns
│   ├── src/                              # Source code
│   │   ├── Api/
│   │   │   └── Booking.Api/              # Web API entry point
│   │   │       ├── Program.cs            # Application bootstrap
│   │   │       ├── Booking.Api.csproj    # Project dependencies
│   │   │       ├── Dockerfile            # Container configuration
│   │   │       ├── appsettings.json      # Application settings
│   │   │       ├── Extensions/           # Service registration extensions
│   │   │       ├── Middlewares/          # Custom middleware components
│   │   │       ├── Services/             # API-specific services
│   │   │       └── TestProfileSeeder.cs  # Development data seeder
│   │   ├── Common/
│   │   │   └── Booking.Common/           # Shared kernel module
│   │   │       ├── Authentication/       # JWT & claims management
│   │   │       │   ├── ClaimsIdentifiers.cs        # Standard claim definitions
│   │   │       │   ├── ClaimsPrincipalExtensions.cs # Claims helper methods
│   │   │       │   ├── UserContext.cs              # Current user abstraction
│   │   │       │   └── EmailVerificationLinkFactory.cs # Email link generation
│   │   │       ├── Authorization/        # Permission-based authorization
│   │   │       │   ├── HasPermissionAttribute.cs         # Permission attribute
│   │   │       │   ├── PermissionAuthorizationHandler.cs # Authorization handler
│   │   │       │   ├── PermissionProvider.cs             # Permission registry
│   │   │       │   └── PermissionRequirement.cs          # Auth requirements
│   │   │       ├── Behaviors/            # MediatR pipeline behaviors
│   │   │       │   ├── ValidationDecorator.cs     # Input validation
│   │   │       │   └── LoggingDecorator.cs        # Request logging
│   │   │       ├── Contracts/            # Cross-module contracts
│   │   │       │   └── Users/            # User module contracts
│   │   │       ├── Domain/               # Domain building blocks
│   │   │       │   └── Entity/           # Base entity classes
│   │   │       ├── Email/                # Email service infrastructure
│   │   │       │   ├── AwsSesEmailService.cs      # AWS SES implementation
│   │   │       │   ├── EmailTemplateProvider.cs   # Template management
│   │   │       │   └── Templates/                 # HTML email templates
│   │   │       ├── Endpoints/            # Minimal API abstractions
│   │   │       ├── Exceptions/           # Custom exception types
│   │   │       ├── Messaging/            # CQRS message contracts
│   │   │       │   ├── ICommand.cs       # Command interface
│   │   │       │   ├── IQuery.cs         # Query interface
│   │   │       │   └── ICommandHandler.cs # Handler interfaces
│   │   │       ├── Options/              # Configuration options
│   │   │       ├── RealTime/             # SignalR infrastructure
│   │   │       │   ├── NotificationHub.cs        # SignalR hub
│   │   │       │   └── NotificationService.cs    # Real-time notifications
│   │   │       ├── Results/              # Result pattern implementation
│   │   │       │   ├── Result.cs         # Success/failure results
│   │   │       │   ├── Error.cs          # Error definitions
│   │   │       │   └── ValidationError.cs # Validation errors
│   │   │       ├── Serialization/        # JSON serialization settings
│   │   │       ├── SlugGenerator/        # URL slug utilities
│   │   │       ├── Uploads/              # File upload & AWS S3
│   │   │       │   └── S3ImageProcessingService.cs # Image processing
│   │   │       ├── ValueObject.cs        # Base value object class
│   │   │       └── ApplicationConfiguration.cs    # App configuration
│   │   └── Modules/                      # Business modules
│   │       ├── Booking.Modules.Users/    # User management module
│   │       │   ├── UsersModule.cs        # Module registration
│   │       │   ├── JwtOptions.cs         # JWT configuration
│   │       │   ├── TokenProvider.cs      # Token generation
│   │       │   ├── TokenWriterCookies.cs # Cookie handling
│   │       │   ├── AssemblyReference.cs  # Module assembly reference
│   │       │   ├── BackgroundJobs/       # Async job processing
│   │       │   │   ├── Cleanup/          # Data cleanup jobs
│   │       │   │   ├── SendingVerificationEmail/ # Email verification
│   │       │   │   └── SendingPasswordResetToken/ # Password reset
│   │       │   ├── Contracts/            # Module contracts
│   │       │   ├── Domain/               # User domain logic
│   │       │   │   ├── Entities/         # User, Profile entities
│   │       │   │   ├── ValueObjects/     # Email, Name value objects
│   │       │   │   ├── Services/         # Domain services
│   │       │   │   ├── UserErrors.cs     # Domain errors
│   │       │   │   └── UserConstraints.cs # Domain constraints
│   │       │   ├── Features/             # User feature implementations
│   │       │   │   ├── Authentication/   # Login, JWT, Google OAuth
│   │       │   │   ├── GetUser/          # User retrieval
│   │       │   │   ├── ImageProcessor/   # Profile image processing
│   │       │   │   └── UsersEndpoints.cs # HTTP endpoints
│   │       │   ├── Persistence/          # Data persistence layer
│   │       │   │   ├── UsersDbContext.cs # EF Core context
│   │       │   │   ├── UnitOfWork.cs     # Unit of Work pattern
│   │       │   │   ├── Migrations/       # Database migrations
│   │       │   │   └── UsersDbContextFactory.cs # Context factory
│   │       │   └── RecurringJobs/        # Scheduled maintenance
│   │       └── Booking.Modules.Catalog/  # Catalog management module
│   │           ├── CatalogModule.cs      # Module registration
│   │           ├── AssemblyReference.cs  # Module assembly reference
│   │           ├── MockKonnectController.cs # Mock external service
│   │           ├── BackgroundJobs/       # Catalog background processing
│   │           ├── Domain/               # Catalog domain logic
│   │           │   ├── Entities/         # Product, Store, Order entities
│   │           │   │   ├── Sessions/     # Session-related entities
│   │           │   │   │   ├── SessionProduct.cs     # Session products
│   │           │   │   │   ├── SessionAvailability.cs # Time slots
│   │           │   │   │   └── BookedSession.cs      # Booked sessions
│   │           │   │   ├── Store.cs      # Store aggregate
│   │           │   │   ├── Product.cs    # Product entity
│   │           │   │   ├── Order.cs      # Order aggregate
│   │           │   │   ├── Payment.cs    # Payment entity
│   │           │   │   ├── Payout.cs     # Payout entity
│   │           │   │   ├── Wallet.cs     # Wallet entity
│   │           │   │   ├── Day.cs        # Calendar day entity
│   │           │   │   └── Escrow.cs     # Escrow entity
│   │           │   └── ValueObjects/     # Catalog value objects
│   │           │       ├── Duration.cs   # Time duration
│   │           │       ├── TimeRange.cs  # Time ranges
│   │           │       ├── OrderStatus.cs # Order states
│   │           │       ├── Picture.cs    # Image handling
│   │           │       ├── MeetLink.cs   # Meeting links
│   │           │       ├── SocialLink.cs # Social media links
│   │           │       └── Checkout.cs   # Checkout information
│   │           ├── Features/             # Catalog feature implementations
│   │           │   ├── Products/         # Product CRUD operations
│   │           │   ├── Stores/           # Store management
│   │           │   ├── Orders/           # Order processing
│   │           │   ├── Payment/          # Payment integration
│   │           │   ├── Payout/           # Vendor payouts
│   │           │   ├── Integrations/     # External integrations
│   │           │   │   └── GoogleCalendar/ # Google Calendar sync
│   │           │   ├── ImageProcessor/   # Product image processing
│   │           │   ├── CatalogEndpoints.cs # HTTP endpoints
│   │           │   └── Utils/            # Utility functions
│   │           └── Persistence/          # Data persistence layer
│   │               ├── CatalogDbContext.cs # EF Core context
│   │               ├── UnitOfWork.cs     # Unit of Work pattern
│   │               ├── Schemas.cs        # Database schemas
│   │               ├── Configurations/   # Entity configurations
│   │               ├── Migrations/       # Database migrations
│   │               ├── Repositories/     # Repository implementations
│   │               └── CatalogDbContextFactory.cs # Context factory
│   └── tests/                            # Test projects
│       └── IntegrationsTests/            # Integration test suite
│           ├── IntegrationsTests.csproj  # Test project configuration
│           ├── Abstractions/             # Test base classes
│           │   └── CatalogTestBase.cs    # Catalog test foundation
│           ├── Mocking/                  # Mock implementations
│           └── Tests/                    # Test implementations
└── README.md                             # This documentation
```

## Modules

### API Module

**Location**: `src/Api/Booking.Api/`

The main entry point and composition root of the application.

#### Key Responsibilities:

- **Application bootstrapping** - Configures services, middleware, and modules
- **Cross-cutting concerns** - Logging, exception handling, health checks
- **Authentication/Authorization** - JWT token validation and permission checks

#### Key Components:

- **Program.cs** - Application entry point and service configuration
- **Extensions/** - Service registration and configuration extensions
- **Middlewares/** - Custom middleware components
- **Services/** - API-specific services and configurations
- **appsettings.json** - Configuration files for different environments

#### Features:

- Health checks for database and external services
- CORS configuration
- Rate limiting
- Request/response logging

### Common Module

**Location**: `src/Common/Booking.Common/`

The shared kernel containing cross-cutting concerns and common functionality used across all modules.

#### Key Responsibilities:

- **Authentication & Authorization** - JWT handling, claims management, permission-based authorization
- **Domain Building Blocks** - Base entities, value objects, domain events
- **Cross-cutting Services** - Email, file uploads, real-time notifications

#### Key Components:

##### Authentication

- **ClaimsIdentifiers.cs** - Standard claim type definitions
- **ClaimsPrincipalExtensions.cs** - Extension methods for claims handling
- **UserContext.cs** - Current user context abstraction
- **EmailVerificationLinkFactory.cs** - Email verification link generation

##### Authorization

- **HasPermissionAttribute.cs** - Permission-based authorization attribute
- **PermissionAuthorizationHandler.cs** - Custom authorization handler
- **PermissionProvider.cs** - Permission registration and management

##### Domain

- **ValueObject.cs** - Base class for domain value objects
- **Entity base classes** - Common entity patterns

##### Infrastructure

- **Endpoints/** - Minimal API endpoint abstractions
- **Behaviors/** - pipeline behaviors
- **Results/** - Result pattern implementations

##### Services

- **Email/** - Email service abstractions and implementations
- **Uploads/** - File upload and AWS S3 integration
- **RealTime/** - SignalR hub abstractions
- **SlugGenerator/** - URL slug generation utilities

### Users Module

**Location**: `src/Modules/Booking.Modules.Users/`

Handles the authentication

#### Key Responsibilities:

- **User Identity Management** - Registration, login, profile updates
- **Authentication Services** - JWT token generation, Google OAuth integration
- **Email Verification** - Account activation and password reset workflows
- **Background Jobs** - Cleanup tasks, email notifications

#### Key Components:

##### Domain

- **Entities/** - User, and related entities
- **UserErrors.cs** - Domain-specific error definitions

##### Features

- **Authentication/** - Login, logout, token refresh, Google OAuth
  - JWT token generation and validation
  - Cookie-based token storage
  - Password reset functionality

##### Infrastructure

- **Persistence/** - Entity Framework configurations and repositories
- **BackgroundJobs/** - Hangfire job implementations
- **RecurringJobs/** - Scheduled maintenance tasks

##### Background Processing

- **SendingVerificationEmail** - Async email verification sending
- **SendingPasswordResetToken** - Password reset email processing
- **Cleanup** - User data maintenance and cleanup

### Catalog Module

**Location**: `src/Modules/Booking.Modules.Catalog/`

Manages the marketplace catalog including products, stores, orders, and payment processing.

#### Key Responsibilities:

- **Product/Service Management** - Create, update, delete marketplace offerings
- **Store Management** - Vendor store creation and management
- **Order Processing** - Order lifecycle management
- **Payment Integration** - Payment processing and payout handling
- **Inventory Management** - Stock tracking and availability
- **Integration Services** - External service integrations (Google Calendar, etc.)

#### Key Components:

##### Domain

- **Entities/** - Product, Store, Order, Payment entities and aggregates
- **ValueObjects/** - Price, Address, ProductDetails value objects

##### Features

- **Products/** - Product CRUD operations, search, filtering
  - Product creation and management
  - Category and tag management
  - Pricing and availability
- **Stores/** - Store management for vendors
  - Store registration and verification
  - Store analytics and reporting
- **Orders/** - Order processing workflow
  - Order creation and tracking
  - Status management
  - Order fulfillment
- **Payment/** - Payment processing integration
  - Multiple payment provider support
  - Transaction tracking
- **Payout/** - Vendor payout processing
  - Automated payout calculations
  - Payout scheduling and tracking
- **Integrations/** - External service integrations
  - **GoogleCalendar/** - Calendar integration for bookings
- **ImageProcessor/** - Product image handling and optimization

##### Infrastructure

- **Persistence/** - Entity Framework configurations for catalog entities
- **BackgroundJobs/** - Catalog-related background processing

#### Command and Query Interfaces

**Commands** - Represent operations that change system state:

```csharp
// Base command interfaces
public interface ICommand;                    // No return value
public interface ICommand<TResponse>;         // With return value

// Example command
public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
```

**Queries** - Represent read operations:

```csharp
// Base query interface
public interface IQuery<TResponse>;

// Example query
public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
```

#### Command and Query Handlers

**Command Handlers** - Process commands and return Results:

```csharp


// Example implementation
public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // Business logic implementation
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return Result.Failure<LoginResponse>(UserErrors.IncorrectEmailOrPassword);

        // ... validation and processing

        return Result.Success(new LoginResponse(token, user));
    }
}
```

**Query Handlers** - Process queries and return Results:

```csharp
public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
```

### Minimal API Endpoint Pattern

#### Endpoint Interface

```csharp
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

#### Endpoint Implementation Pattern

Each feature implements its own endpoint following this structure:

```csharp
// Location: Features/Authentication/Login/Login.cs
internal sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.Login, async (
                Request request,
                ICommandHandler<LoginCommand, LoginResponse> handler,
                CancellationToken cancellationToken = default) =>
            {
                // 1. Map HTTP request to Command
                var command = new LoginCommand(request.Email, request.Password);

                // 2. Execute command via handler
                var result = await handler.Handle(command, cancellationToken);

                // 3. Map Result to HTTP response
                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
    }

    public sealed record Request(string Email, string Password);
}
```

### FluentValidation Integration

The application uses **FluentValidation** for request validation with automatic integration into the pipeline ( middleware for endpoints ).

#### Validator Implementation

Each command has its own validator:

```csharp
// LoginCommandValidator.cs
internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email address is required");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters");
    }
}
```

#### Automatic Validation Registration

Validators are automatically discovered and registered:

```csharp
// In module registration
services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
```

### Custom Results & Validation Decorator

**CustomResults**: `Booking.Common.Results`

The `CustomResults` class provides a unified way to return problem details (`IResult`) from API endpoints, following the RFC 7807 Problem Details for HTTP APIs standard.

- Returns ProblemDetails JSON with:

  - `title` → based on `Error.Code`
  - `detail` → human-readable `Error.Description`
  - `type` → link to RFC specification for the error type
  - `status` → HTTP status code (400, 404, 409, 401, 500...)
  - `extensions` → includes validation errors if present

  #### Example Error Responses

##### 1. Validation Error Response

When a request fails validation (e.g., missing required fields, invalid format), the `ValidationDecorator` returns a `ValidationError`. The `CustomResults.Problem` method serializes it into a 400 Bad Request problem response:

```json
{
  "title": "InvalidInput",
  "detail": "One or more validation errors occurred.",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "status": 400,
  "errors": {
    "Email": [
      "Email must not be empty.",
      "Email must be a valid email address."
    ],
    "Password": ["Password must be at least 8 characters long."]
  }
}
```

**Notes**:

- `title` → comes from the error code
- `detail` → short description
- `errors` → dictionary of property names and their validation messages

##### 2. Not Found Error Response

When a requested resource doesn't exist, a 404 Not Found response is returned:

```json
{
  "title": "User.NotFound",
  "detail": "The user with the given ID was not found.",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "status": 404
}
```

- **CustomResults.Problem** - Converts failed `Result` objects into consistent HTTP Problem responses
- **ValidationDecorator** - Ensures all commands pass validation before being handled, returning structured validation errors when they fail

#### Error Handling System

```csharp
public record Error
{
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

    // Factory methods for different error types
    public static Error Failure(string code, string description);     // 500
    public static Error Problem(string code, string description);     // 400
    public static Error NotFound(string code, string description);    // 404
    public static Error Conflict(string code, string description);    // 409
    public static Error Unauthorized(string code, string description); // 401
}
```

**Error Types** - Categorizes errors for proper HTTP status mapping:

```csharp
public enum ErrorType
{
    Failure = 0,        // 500 Internal Server Error
    Validation = 1,     // 400 Bad Request (validation)
    Problem = 2,        // 400 Bad Request (business rule)
    NotFound = 3,       // 404 Not Found
    Conflict = 4,       // 409 Conflict
    Unauthorized = 5    // 401 Unauthorized
}
```

### Custom Error System

The application defines domain-specific errors for each module, providing consistent error handling across the system.

#### Domain Error Definitions

**Users Module Errors:**

```csharp
public static class UserErrors
{
    public static readonly Error IncorrectEmailOrPassword = Error.Problem(
        "User.IncorrectEmailOrPassword",
        "The provided email or password is incorrect");

    public static readonly Error AccountLockedOut = Error.Problem(
        "User.AccountLockedOut",
        "The account is temporarily locked due to failed login attempts");

    public static readonly Error EmailAlreadyExists = Error.Conflict(
        "User.EmailAlreadyExists",
        "A user with this email address already exists");

    public static readonly Error NotFound = Error.NotFound(
        "User.NotFound",
        "The user was not found");
}
// using it
return Result.Failure<LoginResponse>(UserErrors.AccountLockedOut);

```

### Pipeline Behaviors

The application uses **pipeline behaviors** to implement cross-cutting concerns.

#### Validation Behavior

Automatically validates commands before execution:

```csharp
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Validate request
        var validationResult = await ValidateAsync(request);

        if (!validationResult.IsValid)
            return CreateValidationFailureResponse<TResponse>(validationResult);

        // Continue to next behavior or handler
        return await next();
    }
}
```

#### Logging Behavior

Logs request/response information:

```csharp
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

        var response = await next();

        _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
```

### Local Development Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd booking_monorepo/backend
   ```

2. **Navigate to the solution**

   ```bash
   cd Booking
   ```

3. **Restore dependencies**

   ```bash
   dotnet restore
   ```

4. **Configure database connection**
   Update `appsettings.Development.json` in `src/Api/Booking.Api/`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=booking_db;Username=your_user;Password=your_password"
     }
   }
   ```

5. **Run database migrations**

   ```bash
   dotnet ef database update --project src/Api/Booking.Api
   ```

6. **Run the application**
   ```bash
   dotnet run --project src/Api/Booking.Api
   ```

<!-- ### Docker Development

1. **Use Docker Compose** (from repository root):
   ```bash
   docker-compose -f docker-compose.dev.yml up
   ``` -->

### Module Communication

- **Shared contracts** defined in Common module

### Database Migrations

```bash
# Add migration for Users module
dotnet ef migrations add MigrationName --project src/Modules/Booking.Modules.Users --context UsersDbContext

# Add migration for Catalog module
dotnet ef migrations add MigrationName --project src/Modules/Booking.Modules.Catalog --context CatalogDbContext
```

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run integration tests only
dotnet test tests/IntegrationsTests/

```

### Test Structure

- **Integration Tests** - Test module integration and database operations
- **Test Base Classes** - Shared test infrastructure in `tests/IntegrationsTests/Abstractions/`

### Test Categories

- **CatalogTestBase.cs** - Base class for catalog-related tests
- **Mocking/** - Mock implementations for external dependencies
- **Tests/** - Organized by module and feature

---

## Configuration

Key configuration areas:

- **Authentication** - JWT settings, Google OAuth credentials
- **Database** - Connection strings and EF Core settings
- **Email** - SMTP settings for email notifications
- **AWS** - S3 configuration for file uploads
- **Hangfire** - Background job dashboard and processing
- **Logging** - Serilog configuration for structured logging

For detailed configuration options, refer to the `appsettings.json` files in each project.
