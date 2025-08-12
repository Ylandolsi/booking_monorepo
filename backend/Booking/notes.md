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

DB :
psql -h localhost -p 5432 -U postgres -d clean-architecture
sudo apt update
sudo apt install postgresql-client

```bash

# ---------------------------
# Docker Commands to Know
# ---------------------------

# 1. Check running containers
docker ps

# 2. Check all containers (including stopped)
docker ps -a

# 3. Start a container by name or ID
docker start <container_name_or_id>

# 4. Stop a container by name or ID
docker stop <container_name_or_id>

# 5. Remove a container by name or ID
docker rm <container_name_or_id>

# 6. View logs of a container
docker logs <container_name_or_id>

# 7. Run a Postgres container with port mapping and password set
docker run -d --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres:latest

# 8. Execute an interactive shell inside a running container
docker exec -it <container_name_or_id> bash


# ---------------------------
# PostgreSQL psql Commands to Know
# ---------------------------

# 1. Connect to Postgres database (replace user, db, and port accordingly)
psql -h localhost -p 5432 -U postgres -d clean-architecture

# 2. List all databases
\l

# 3. Connect to a different database
\c database_name

# 4. List tables in the current database
\dt

# 5. Describe table structure
\d table_name

# 6. Run an SQL query (example)
SELECT * FROM table_name;
SELECT * FROM "AspNetUsers";

Why the error?
SELECT * FROM AspNetUsers;
is interpreted as:

SELECT * FROM aspnetusers;
// (which doesn’t exist).

# 7. Exit the psql shell
\q


# ---------------------------
# Docker Compose Commands
# ---------------------------

# Build and start services defined in docker-compose.yml
docker compose up --build


# ---------------------------
# Inspect container names and their published ports
# ---------------------------

docker inspect -f '{{ .Name }} - {{ range .NetworkSettings.Ports }}{{ println . }}{{ end }}' $(docker ps -q)


# ---------------------------
# Check and free a port (example: port 8081)
# ---------------------------

# List processes using port 8081
sudo lsof -i :8081

# Manually kill docker-proxy or other processes blocking port 8081
sudo kill 50559 50568

# Check again if port is free
sudo lsof -i :8081

# If still not free, force kill the processes
sudo kill -9 50559 50568

# Confirm port 8081 is now free (no output means free)
sudo lsof -i :8081

```

clean-architecture=# SELECT *
FROM "users"."AspNetUsers"
WHERE "slug" = 'yassine-landolsi';
clean-architecture=# 




TimeSlots = new object[0]  : empty object 

var availability = await response.Content.ReadFromJsonAsync<dynamic>();
Assert.NotNull(availability);
Assert.True(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
Assert.True(((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength() > 0);


---

var availabilityWeekTasks = Enumerable.Range(0, 7)
.Select(async dayOfWeek =>
{ do something .. return .. } = 

        var availabilityWeek = (await Task.WhenAll(availabilityWeekTasks)).ToList();
