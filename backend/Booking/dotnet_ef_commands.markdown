# .NET and EF Core Development Guide

## External Resources

### Documentation & Tools

- [Claude AI Chat](https://claude.ai/chat/a5dd9237-52e8-49c8-88e6-703768f4caaf)
- [Google AlloyDB](https://cloud.google.com/products/alloydb?hl=en)
- [Calendar Component](https://chriscooper0.github.io/calendar/)

## EF Core Configuration Notes

### Entity Inheritance & Navigation Properties

**Store inheritance pattern**: SessionProducts implement Products and appear in the navigation property.

### Enum Storage as Strings

To save enums as strings in the database, configure EF Core explicitly:

```csharp
modelBuilder
    .Entity<Payout>()
    .Property(p => p.Status)
    .HasConversion<string>();
```

This ensures:

- **DB column** → `"Pending"` / `"Completed"`
- **JSON output** → `"Pending"` / `"Completed"`

### Property Naming Convention

In EF Core, if you don't configure anything, it uses your C# property names (usually PascalCase).

### Table-Level Constraints

Add database-level validation constraints:

```csharp
// Add table-level constraints
builder.ToTable("sessions", t =>
{
    t.HasCheckConstraint("CK_Session_Price_Positive", "price_amount > 0");
    t.HasCheckConstraint("CK_Session_Duration_Valid", "duration_minutes > 0 AND duration_minutes <= 480"); // Max 8 hours
    t.HasCheckConstraint("CK_Session_Date_Valid", "scheduled_at > created_at");
});
```

### Row-Level Locking

Check wallet balance with row-level locking:

```csharp
var wallet = await _dbContext.Wallets
    .Where(w => w.UserId == request.UserId)
    .ExecuteUpdateAsync(w => w.SetProperty(x => x.Balance, x => x.Balance), cancellationToken);
```

## Cancellation Tokens

### Request Aborted vs Cancellation Token

| Aspect            | `HttpContext.RequestAborted`              | `cancellationToken` (method param)                                               |
| ----------------- | ----------------------------------------- | -------------------------------------------------------------------------------- |
| **Scope**         | Only for the current HTTP request         | General-purpose, can be chained or created manually                              |
| **Triggered by**  | Client disconnects, request times out     | Whatever cancellation policy you define (timeouts, linked tokens, manual cancel) |
| **Control**       | Framework-controlled                      | You (developer) decide                                                           |
| **Best practice** | Use it to stop work when client goes away | Use it for explicit cancellation needs beyond HTTP lifecycle                     |

## Entity Framework Migrations

### Migration Commands by Module

#### Users Module Migration

```bash
dotnet ef migrations add Initial \
  --project src/Modules/Booking.Modules.Users/Booking.Modules.Users.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context UsersDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations
```

#### Catalog Module Migration

```bash
dotnet ef migrations add FixUpdatedAt \
  --project src/Modules/Booking.Modules.Catalog/Booking.Modules.Catalog.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context CatalogDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations
```

#### Mentorships Module Migration

```bash
dotnet ef migrations add MentorInitial \
  --project src/Modules/Booking.Modules.Mentorships/Booking.Modules.Mentorships.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context MentorshipsDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations
```

### Alternative Migration Commands

#### Sync Model and Database

```bash
dotnet ef migrations add SyncModelAndDb --project ../Infrastructure --startup-project .
```

#### Initial Mentorships Migration (Alternative)

```bash
cd /path/to/mentorships/module
dotnet ef migrations add InitialMentorshipsMigration --startup-project ../Api/Booking.Api
```

### EF Core Parameters Explained

- **`--project`**: Specifies where migration files are generated (e.g., Users or Mentorships module)
- **`--startup-project`**: Points to the project with the DI container setup (e.g., API project)
- **`--context`**: Specifies the DbContext class to use
- **`--configuration`**: Build configuration (Debug/Release)
- **`--output-dir`**: Directory where migration files will be created

**Note**: EF Tools use the API project's `Program.cs` to build services and create the `DbContext`.

## Testing

### Running Specific Tests

Run a specific test with detailed output:

```bash
dotnet test tests/IntegrationsTests/IntegrationsTests.csproj \
  --filter "CreateStore_ShouldSucceed_WhenValidDataProvided" \
  -v d
```

## Development Notes

### JSON Serialization

**System.Text.Json vs Newtonsoft.Json**:

- `System.Text.Json` is case-insensitive by default
- `Newtonsoft.Json` is case-sensitive

**EF Core Entity Serialization**:
When returning EF Core entities directly, `System.Text.Json` serializes every public getter (mapped or not), so properties like `DomainEvents` may appear in the output despite not being database columns.

### Background Jobs in Tests

In integration tests, background jobs need to be triggered manually because they are not automatically executed in the test environment.

### PolyJSON

_Note: Additional JSON handling utilities and patterns._

## Quick Reference

### Common EF Core Commands

```bash
# Add migration
dotnet ef migrations add <MigrationName> --project <ModuleProject> --startup-project <ApiProject> --context <DbContext>

# Update database
dotnet ef database update --project <ModuleProject> --startup-project <ApiProject> --context <DbContext>

# Remove last migration
dotnet ef migrations remove --project <ModuleProject> --startup-project <ApiProject> --context <DbContext>

# List migrations
dotnet ef migrations list --project <ModuleProject> --startup-project <ApiProject> --context <DbContext>
```

### Module Contexts

- **Users**: `UsersDbContext`
- **Catalog**: `CatalogDbContext`
- **Mentorships**: `MentorshipsDbContext`
