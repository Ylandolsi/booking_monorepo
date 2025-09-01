# .NET and EF Core Commands

In EF Core, if you don’t configure anything, it uses your C# property names (usually PascalCase).

Check wallet balance with row-level locking

```c#
 var wallet = await _dbContext.Wallets.
    Where(w => w.UserId == request.UserId).
    ExecuteUpdateAsync(w => w.SetProperty(x => x.Balance, x => x.Balance), cancellationToken);
```

## Entity Framework Migrations

Add migrations for the Users module:

```bash
dotnet ef migrations add Initial \
  --project src/Modules/Booking.Modules.Users/Booking.Modules.Users.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context UsersDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations
```

Add migrations for the Mentorships module:

```bash
dotnet ef migrations add MentorInitial \
  --project src/Modules/Booking.Modules.Mentorships/Booking.Modules.Mentorships.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context MentorshipsDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations
```

Add migration to sync model and database:

```bash
dotnet ef migrations add SyncModelAndDb --project ../Infrastructure --startup-project .
```

Add initial migration for the Mentorships module:

```bash
cd /path/to/mentorships/module
dotnet ef migrations add InitialMentorshipsMigration --startup-project ../Api/Booking.Api
```

## Notes on EF Core Parameters

- The `--project` parameter specifies where migration files are generated (e.g., Users or Mentorships module).
- The `--startup-project` parameter points to the project with the DI container setup (e.g., API project).
- EF Tools use the API project's `Program.cs` to build services and create the `DbContext`.

## JSON Serialization Notes

- `System.Text.Json` is case-insensitive by default, but `Newtonsoft.Json` is case-sensitive.
- When returning EF Core entities directly, `System.Text.Json` serializes every public getter (mapped or not), so properties like `DomainEvents` may appear in the output despite not being database columns.

## Background Jobs in Tests

In integration tests, background jobs need to be triggered manually because they are not automatically executed in the test environment.
