Yasminelandolsi: fix this notes

GitHub Copilot: To fix the notes, correct typos, formatting, and code block usage for clarity and accuracy.

```bash
  dotnet tool install --global dotnet-ef
```

PostgreSQL's timestamp with time zone requires UTC DateTime values.

```
ensure UTC
parsedDate = DateTime.SpecifyKind(parsedDate.Date, DateTimeKind.Utc);
```

````
```bash
dotnet ef migrations add Initial \
  --project src/Modules/Booking.Modules.Users/Booking.Modules.Users.csproj \
  --startup-project src/Api/Booking.Api/Booking.Api.csproj \
  --context UsersDbContext \
  --configuration Debug \
  --output-dir Persistence/Migrations


   dotnet ef migrations add MentorInitial     --project src/Modules/Booking.Modules.Mentorships/Booking.Modules.Mentorships.csproj     --startup-project src/Api/Booking.Api/Booking.Api.csproj     --context MentorshipsDbContext     --configuration Debug     --output-dir Persistence/Migrations


````

- The `--project` parameter specifies where to put the migration files (Users module).
- The `--startup-project` parameter specifies which project has the DI container setup (API project).
- This way, EF Tools can use the API project's `Program.cs` to build the services and create the `DbContext`.

## In integration tests, background jobs need to be triggered manually because they are not automatically executed in the test environment.

```csharp
// Ensure the token is properly encoded for URL usage (+, /, =, etc.)
string encodedToken = Uri.EscapeDataString(emailVerificationToken);
string verificationLink = $"{_frontEndOptions.BaseUrl}{_frontEndOptions.EmailVerificationPagePath}?token={encodedToken}&email={emailAddress}";
return verificationLink ?? throw new InvalidOperationException("Failed to generate verification link.");
```

```bash
dotnet ef migrations add SyncModelAndDb --project ../Infrastructure --startup-project .
```

- `System.Text.Json` is case-insensitive by default, but `Newtonsoft.Json` is case-sensitive.

When you return your EF Core entities directly, `System.Text.Json` will serialize every public getter—mapped or not—so
your `DomainEvents` list shows up even though it isn’t a database column.

```csharp
[NotMapped] // EF Core will ignore it (optional, since it’s already not mapped)
[JsonIgnore] // JSON serializer will skip it
public List<IDomainEvent> DomainEvents => _domainContainer.DomainEvents;
```

To retrieve services:

```csharp
var userContext = context.RequestServices.GetRequiredService<IUserContext>();
// or
using var scope = app.Services.CreateScope();
var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
// or
private readonly IServiceProvider _serviceProvider;
using var scope = _serviceProvider.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
```

```bash
dotnet build --verbosity detailed
dotnet build --no-restore
dotnet build 2>&1 | grep -E "(error|Error)" | head -10
```

```bash
cd /path/to/mentorships/module
dotnet ef migrations add InitialMentorshipsMigration --startup-project ../Api/Booking.Api
```

            $"{Base}/availability/month?mentorSlug={Uri.EscapeDataString(mentorSlug)}&year={year}&month={month}";

```csharp
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // ignore circular references in JSON serialization 
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
```