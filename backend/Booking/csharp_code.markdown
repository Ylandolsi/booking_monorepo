# C# Code Snippets

-         using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
-         var httpClient = httpClientFactory.CreateClient();

## Handling UTC DateTime for PostgreSQL

Ensure DateTime values are in UTC for PostgreSQL's `timestamp with time zone`:

```csharp
parsedDate = DateTime.SpecifyKind(parsedDate.Date, DateTimeKind.Utc);
```

## Generating Verification Links

Properly encode tokens for URL usage:

```csharp
string encodedToken = Uri.EscapeDataString(emailVerificationToken);
string verificationLink = $"{_frontEndOptions.BaseUrl}{_frontEndOptions.EmailVerificationPagePath}?token={encodedToken}&email={emailAddress}";
return verificationLink ?? throw new InvalidOperationException("Failed to generate verification link.");
```

## Ignoring Properties in JSON Serialization

Prevent serialization of non-mapped properties in EF Core entities:

```csharp
[NotMapped] // EF Core will ignore it (optional, since it’s already not mapped)
[JsonIgnore] // JSON serializer will skip it
public List<IDomainEvent> DomainEvents => _domainContainer.DomainEvents;
```

## Retrieving Services in ASP.NET Core

Access services via dependency injection:

```csharp
// Using HttpContext
var userContext = context.RequestServices.GetRequiredService<IUserContext>();

// Using IServiceScope
using var scope = app.Services.CreateScope();
var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();

// Using IServiceProvider
private readonly IServiceProvider _serviceProvider;
using var scope = _serviceProvider.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
```

## JSON Serialization Configuration

Configure JSON serialization to ignore circular references:

```csharp
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Ignore circular references in JSON serialization
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
```

## Handling API Responses

Process dynamic JSON responses and assert conditions:

```csharp
var availability = await response.Content.ReadFromJsonAsync<dynamic>();
Assert.NotNull(availability);
Assert.True(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
Assert.True(((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength() > 0);
```

## Asynchronous Task Processing

Process a week’s availability in parallel:

```csharp
var availabilityWeekTasks = Enumerable.Range(0, 7)
    .Select(async dayOfWeek =>
    {
        // Do something
        // Return result
    });
var availabilityWeek = (await Task.WhenAll(availabilityWeekTasks)).ToList();
```

## URL Formatting

Format URLs with proper encoding:

```csharp
$"{Base}/availability/month?mentorSlug={Uri.EscapeDataString(mentorSlug)}&year={year}&month={month}";
```

## Empty Object Initialization

Initialize an empty array of objects:

```csharp
TimeSlots = new object[0]; // Empty object array
```