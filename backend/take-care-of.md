**Additional Concerns:**

- Role name inconsistency: code uses `"admin"` (lowercase) but you create `"Admin"` (capitalized) - this is case-sensitive!

---

---

### 5. **Database Connection String Exposed**

**File:** `src/Api/Booking.Api/appsettings.Development.json`

**Issue:**

```json
{
  "ConnectionStrings": {
    "Database": "Host=postgres;Port=5432;Database=clean-architecture;Username=postgres;Password=postgres;Include Error Detail=true"
  }
}
```

**Problems:**

- Hardcoded credentials in configuration file
- `Include Error Detail=true` should NOT be in production (security risk)
- Database credentials should be in secrets/environment variables

**Fix:**

```csharp
// Program.cs - Read from environment variables
var connectionString = builder.Configuration.GetConnectionString("Database")
    ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
    ?? throw new InvalidOperationException("Database connection string not configured");
```

**Production Configuration:**

```bash
# Environment variable
DATABASE_CONNECTION_STRING="Host=prod-db.example.com;Port=5432;Database=booking_prod;Username=app_user;Password=<secure-password>;Pooling=true;Minimum Pool Size=5;Maximum Pool Size=100;Connection Lifetime=300;Command Timeout=30"
```

---

---

---

**Problem:**
The `KonnectService` is registered as `Scoped`, but I see it's used in background jobs that are also `Scoped`. This is correct, but we need to verify:

1. Does `KonnectService` use `IHttpContextAccessor`? If yes, it should be Scoped ✅
2. Does it maintain state? If no, it could be Singleton for better performance
3. Is it thread-safe? Background jobs may run concurrently

**Review Needed:**
Check `KonnectService` implementation to verify the correct lifetime.

---

---

### 12. **HTTP Client Timeout Too High**

**File:** `Services/Infrastructure.cs` (line 113)

**Issue:**

```csharp
services.AddHttpClient("KonnectClient", client =>
{
    client.BaseAddress = new Uri(konnectOptions.ApiUrl);
    client.DefaultRequestHeaders.Add("x-api-key", konnectOptions.ApiKey);
    client.Timeout = TimeSpan.FromSeconds(300);  // ❌ 5 minutes!
})
```

**Problem:**

- 300 seconds (5 minutes) is way too high
- Will cause thread pool exhaustion under load
- User requests will hang for too long

**Recommendation:**

```csharp
client.Timeout = TimeSpan.FromSeconds(30);  // ✅ 30 seconds max

// Or from configuration
client.Timeout = TimeSpan.FromSeconds(
    konnectOptions.TimeoutSeconds ?? 30);
```

---

---

### 14. **Channel Configuration - Potential Message Loss**

**File:** `CatalogModule.cs` (lines 52-54)

**Issue:**

```csharp
services.AddSingleton(_ => Channel.CreateBounded<StoreVisit>(new BoundedChannelOptions(1000)
{
    FullMode = BoundedChannelFullMode.Wait  // ❌ Blocks when full
}));
```

**Problem:**

- When channel is full (1000 items), new writes will block
- This will block HTTP requests trying to log visits
- Could cause request timeouts and poor user experience

**Better Options:**

```csharp
// Option 1: Drop oldest (if losing old visits is acceptable)
services.AddSingleton(_ => Channel.CreateBounded<StoreVisit>(
    new BoundedChannelOptions(1000)
    {
        FullMode = BoundedChannelFullMode.DropOldest
    }));

// Option 2: Drop newest (preferred if older data is more valuable)
services.AddSingleton(_ => Channel.CreateBounded<StoreVisit>(
    new BoundedChannelOptions(1000)
    {
        FullMode = BoundedChannelFullMode.DropNewest
    }));

// Option 3: Increase capacity and log warnings
services.AddSingleton(_ => Channel.CreateBounded<StoreVisit>(
    new BoundedChannelOptions(10000)  // Higher limit
    {
        FullMode = BoundedChannelFullMode.DropOldest
    }));
```

Also improve the warning logic in `StoreVisitChannel`:

```csharp
public async ValueTask QueueVisitAsync(Domain.Entities.StoreVisit visit)
{
    if (!_channel.Writer.TryWrite(visit))
    {
        _logger.LogWarning(
            "Channel is full (capacity: {Capacity}), dropping store visit for store: {StoreSlug}. " +
            "Consider increasing channel capacity or processing frequency.",
            1000, // Or get from config
            visit.StoreSlug);

        // Increment a metric here for monitoring
    }
}
```

---

---

## 📋 Pre-Production Checklist

### Security

- [ ] Move all secrets to environment variables/Key Vault
- [ ] Enable HTTPS enforcement
- [ ] Set up rate limiting
- [ ] Review role names (Admin vs admin inconsistency)
- [ ] Disable detailed error messages in production
- [ ] Remove `Include Error Detail=true` from connection strings

### Performance

- [ ] Add `.AsNoTracking()` to all read-only queries
- [ ] Add recommended database indexes
- [ ] Review HTTP client timeouts
- [ ] Implement Redis distributed cache
- [ ] Configure connection pooling
- [ ] Review Channel capacity and behavior
- [ ] Enable response compression
- [ ] Configure output caching where appropriate

### Reliability

- [ ] Fix Hangfire authorization scope leak
- [ ] Add transactions to all background jobs
- [ ] Add circuit breakers for external services
- [ ] Implement retry policies for transient failures

### Monitoring & Observability

- [ ] Configure Application Insights or similar APM
- [ ] Set up alerting for critical errors
- [ ] Add performance counters
- [ ] Configure health check endpoints
- [ ] Set up log aggregation (ELK, Seq, etc.)
- [ ] Add custom metrics for business events
- [ ] Configure distributed tracing

### Database

- [ ] Create all recommended indexes
- [ ] Set up automated backups
- [ ] Configure connection pooling parameters
- [ ] Test failover scenarios
- [ ] Review query execution plans
- [ ] Set up database monitoring

### Configuration

- [ ] Create production appsettings
- [ ] Document all required environment variables
- [ ] Set up configuration validation on startup
- [ ] Remove all hardcoded values
- [ ] Review timeout and retry configurations

### Infrastructure

- [ ] Set up container resource limits
- [ ] Configure auto-scaling rules
- [ ] Set up load balancing
- [ ] Test deployment pipeline
- [ ] Create rollback procedures
- [ ] Document disaster recovery plan

---

---

### **Add Feature Flags**

Consider using feature flags for safer production rollouts:

```csharp
services.AddFeatureManagement();
```
