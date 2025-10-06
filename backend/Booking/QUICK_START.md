# 🚀 Quick Start - Admin Notifications & Health Checks

## Immediate Actions (10 minutes)

### 1. Register Services (Add to your DI container)

```csharp
// In Program.cs or your module registration

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" })
    .AddCheck<HangfireHealthCheck>("hangfire", tags: new[] { "ready" })
    .AddCheck<GoogleCalendarHealthCheck>("google_calendar", tags: new[] { "ready" })
    .AddCheck<KonnectPaymentHealthCheck>("konnect_payment", tags: new[] { "ready" });

// Admin Notification Persistence
builder.Services.AddScoped<IAdminNotificationPersistence, AdminNotificationPersistence>();
```

### 2. Map Endpoints (Add to Program.cs)

```csharp
// After app.Build()

// Health check endpoints
app.MapHealthCheckEndpoints();

// SignalR hub (if not already mapped)
app.MapHub<NotificationHub>("/hubs/notifications");
```

### 3. Run Migrations

```bash
cd backend/Booking/src/Modules/Booking.Modules.Catalog

# Create migration
dotnet ef migrations add AddAdminNotifications --context CatalogDbContext

# Apply migration
dotnet ef database update --context CatalogDbContext
```

### 4. Test Health Checks

```bash
# Start your application, then:
curl http://localhost:5000/health
```

## Expected Response

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "database",
      "status": "Healthy",
      "description": "Database is healthy"
    },
    {
      "name": "hangfire",
      "status": "Healthy",
      "description": "Hangfire is healthy"
    }
  ],
  "timestamp": "2025-10-06T10:30:00Z"
}
```

## Test Admin Notifications

### Trigger a Test Alert (in any service/controller)

```csharp
// Inject NotificationService
private readonly NotificationService _notificationService;

// Send test alert
await _notificationService.SendAdminAlertAsync(
    title: "Test Alert",
    message: "This is a test notification",
    severity: AdminAlertSeverity.Info,
    metadata: new { TestData = "Hello!" }
);
```

### Check Database

```sql
SELECT * FROM catalog.admin_notifications
ORDER BY created_at DESC
LIMIT 10;
```

## Frontend Quick Setup

```typescript
// Install SignalR
// npm install @microsoft/signalr

import { HubConnectionBuilder } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl("/hubs/notifications", {
    accessTokenFactory: () => getToken(),
  })
  .withAutomaticReconnect()
  .build();

// Connect and join admin group
await connection.start();
await connection.invoke("JoinAdminGroup");

// Listen for alerts
connection.on("ReceiveAdminAlert", (alert) => {
  console.log("Admin Alert:", alert);
  // Show notification in your UI
});
```

## Troubleshooting

### Health Check Returns 404

```csharp
// Make sure you called:
app.MapHealthCheckEndpoints();
```

### No Notifications in Database

```csharp
// Verify service registration:
services.AddScoped<IAdminNotificationPersistence, AdminNotificationPersistence>();

// Check NotificationService is registered with persistence:
services.AddScoped<NotificationService>(sp => {
    var hubContext = sp.GetService<IHubContext<NotificationHub>>();
    var logger = sp.GetRequiredService<ILogger<NotificationService>>();
    var persistence = sp.GetService<IAdminNotificationPersistence>(); // <-- Must be included
    return new NotificationService(hubContext, logger, persistence);
});
```

### SignalR Connection Fails

- Check CORS settings
- Verify authentication token
- Ensure user has "Admin" role

## 📚 Full Documentation

See: `ADMIN_NOTIFICATIONS_SETUP.md` for complete details.

## 🎯 Success!

You're ready when:

- ✅ `/health` returns 200 OK
- ✅ Admin notifications table exists
- ✅ Test alert appears in database
- ✅ SignalR connection works
