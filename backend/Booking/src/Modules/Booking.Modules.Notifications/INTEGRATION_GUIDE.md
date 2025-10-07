# Notifications Module - Integration Guide

## Phase 10: Integration & Testing

This guide walks through integrating the Notifications module into the main application.

---

## Step 1: Add Project Reference to API

**File:** `src/Api/Booking.Api.csproj`

Add reference to the Notifications module:

```xml
<ItemGroup>
    <ProjectReference Include="..\Modules\Booking.Modules.Notifications\Booking.Modules.Notifications.csproj" />
</ItemGroup>
```

---

## Step 2: Register Module in Program.cs

**File:** `src/Api/Program.cs`

Add the Notifications module registration:

```csharp
using Booking.Modules.Notifications;

// ... existing code ...

// Add after other module registrations (Users, Catalog)
builder.Services.AddNotificationsModule(builder.Configuration);

// ... existing code ...

var app = builder.Build();

// Configure Hangfire background jobs (after app.UseHangfireDashboard() if applicable)
NotificationsModule.ConfigureBackgroundJobs();

// ... rest of configuration ...
```

---

## Step 3: Run Database Migrations

Generate and apply the notifications schema migration:

```bash
cd src/Modules/Booking.Modules.Notifications

# Generate migration (if not already created)
dotnet ef migrations add InitialCreate --context NotificationsDbContext

# Update database
dotnet ef database update --context NotificationsDbContext --project ../../../Api/Booking.Api.csproj --startup-project ../../../Api/Booking.Api.csproj
```

Or from the solution root:

```bash
dotnet ef database update --context NotificationsDbContext --project src/Modules/Booking.Modules.Notifications/Booking.Modules.Notifications.csproj --startup-project src/Api/Booking.Api.csproj
```

This creates:

- Schema: `notifications`
- Table: `notification_outbox`
- Indexes: 9 optimized indexes for querying

---

## Step 4: Configure Email Settings

**File:** `appsettings.json` or `appsettings.Production.json`

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Database=booking;Username=postgres;Password=yourpassword"
  },
  "Email": {
    "AwsSes": {
      "SenderEmail": "noreply@yourdomain.com",
      "MaxRetryAttempts": 3,
      "RetryDelayMilliseconds": 1000
    }
  },
  "AWS": {
    "Profile": "default",
    "Region": "us-east-1"
  }
}
```

**Important:** Ensure AWS credentials are configured (via AWS CLI, environment variables, or IAM role).

---

## Step 5: Verify Hangfire Configuration

Ensure Hangfire is already configured in `Program.cs`:

```csharp
// Should already exist
builder.Services.AddHangfire(config => config
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddHangfireServer();

// After app is built
app.UseHangfireDashboard("/hangfire");
```

The Notifications module will register two recurring jobs:

1. **process-notification-outbox**: Runs every 2 minutes
2. **cleanup-old-notifications**: Runs daily at 2 AM

---

## Step 6: Test the Integration

### 6.1 Simple Integration Test

Create a test endpoint or use an existing feature:

```csharp
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Abstractions.Contracts;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public TestController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromBody] string recipientEmail)
    {
        var request = new SendEmailRequest
        {
            Recipient = recipientEmail,
            Subject = "Test Email",
            TemplateName = "VerificationEmailForRegistration",
            TemplateData = new
            {
                VERIFICATION_LINK = "https://yourapp.com/verify?token=test123",
                APP_NAME = "Booking App",
                SUPPORT_LINK = "https://yourapp.com/support",
                SECURITY_LINK = "https://yourapp.com/security"
            },
            Priority = NotificationPriority.Normal,
            NotificationReference = $"test-{Guid.NewGuid()}"
        };

        var result = await _notificationService.EnqueueEmailAsync(request);

        if (result.IsSuccess)
        {
            return Ok(new {
                message = "Email queued successfully",
                notificationId = result.NotificationId
            });
        }

        return BadRequest(new { error = result.ErrorMessage });
    }

    [HttpGet("notification-status/{id}")]
    public async Task<IActionResult> GetNotificationStatus(Guid id)
    {
        var status = await _notificationService.GetNotificationStatusAsync(id);

        if (status == null)
        {
            return NotFound();
        }

        return Ok(status);
    }
}
```

### 6.2 Test via Database

```sql
-- Check queued notifications
SELECT * FROM notifications.notification_outbox
WHERE status = 'Pending'
ORDER BY created_at DESC;

-- Check sent notifications
SELECT * FROM notifications.notification_outbox
WHERE status = 'Sent'
ORDER BY sent_at DESC
LIMIT 10;

-- Check failed notifications
SELECT id, recipient, status, attempts, max_attempts, last_error
FROM notifications.notification_outbox
WHERE status = 'Failed';
```

### 6.3 Monitor via Hangfire Dashboard

1. Navigate to `/hangfire` in your browser
2. Check **Recurring Jobs** tab for:
   - `process-notification-outbox` (should run every 2 minutes)
   - `cleanup-old-notifications` (daily at 2 AM)
3. Check **Processing** or **Succeeded** tabs to see job execution history

---

## Step 7: Migrate Existing Email Jobs (Optional)

Update `VerificationEmailForRegistrationJob` in the Users module:

```csharp
// Before
public class VerificationEmailForRegistrationJob
{
    private readonly AwsSesEmailService _emailService;
    private readonly EmailTemplateProvider _emailTemplateProvider;

    public VerificationEmailForRegistrationJob(
        AwsSesEmailService emailService,
        EmailTemplateProvider emailTemplateProvider)
    {
        _emailService = emailService;
        _emailTemplateProvider = emailTemplateProvider;
    }

    public async Task SendAsync(string userEmail, string verificationLink)
    {
        var (subject, body) = await _emailTemplateProvider.GetTemplateAsync(...);
        await _emailService.SendEmailAsync(userEmail, subject, body, cancellationToken);
    }
}

// After
public class VerificationEmailForRegistrationJob
{
    private readonly INotificationService _notificationService;

    public VerificationEmailForRegistrationJob(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task SendAsync(string userEmail, string verificationLink, PerformContext? context)
    {
        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

        var request = new SendEmailRequest
        {
            Recipient = userEmail,
            Subject = "Verify Your Email",
            TemplateName = "VerificationEmailForRegistration",
            TemplateData = new
            {
                VERIFICATION_LINK = verificationLink,
                APP_NAME = "Booking App",
                SUPPORT_LINK = "https://yourapp.com/support",
                SECURITY_LINK = "https://yourapp.com/security"
            },
            NotificationReference = $"verification-{userEmail}",
            Priority = NotificationPriority.High
        };

        var result = await _notificationService.EnqueueEmailAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new Exception($"Failed to queue verification email: {result.ErrorMessage}");
        }

        context?.WriteLine($"Verification email queued with ID: {result.NotificationId}");
    }
}
```

Don't forget to:

1. Add project reference to `Booking.Modules.Notifications.Abstractions`
2. Update using statements
3. Remove dependency on `AwsSesEmailService` and `EmailTemplateProvider`

---

## Step 8: Verify End-to-End Flow

1. **Enqueue Email**:

   ```bash
   curl -X POST https://localhost:5001/api/test/send-test-email \
     -H "Content-Type: application/json" \
     -d '"test@example.com"'
   ```

2. **Check Database**:

   ```sql
   SELECT * FROM notifications.notification_outbox
   WHERE recipient = 'test@example.com'
   ORDER BY created_at DESC LIMIT 1;
   ```

   Status should be `Pending`.

3. **Wait for Outbox Processor** (runs every 2 minutes):

   - Or manually trigger via Hangfire dashboard

4. **Verify Email Sent**:

   ```sql
   SELECT * FROM notifications.notification_outbox
   WHERE recipient = 'test@example.com'
   ORDER BY created_at DESC LIMIT 1;
   ```

   Status should be `Sent`, `sent_at` should be populated.

5. **Check Email Inbox**: Verify the email was received.

---

## Troubleshooting

### Emails Not Sending

1. **Check Hangfire Job Status**:

   - Visit `/hangfire` dashboard
   - Look for `process-notification-outbox` job
   - Check for errors in **Failed** jobs

2. **Check AWS SES Configuration**:

   ```bash
   aws ses verify-email-identity --email-address noreply@yourdomain.com
   aws ses get-send-quota
   ```

3. **Check Application Logs**:

   ```bash
   grep -i "notification\|outbox" logs/app.log
   ```

4. **Manually Process Outbox** (for testing):
   ```sql
   -- Check pending notifications
   SELECT COUNT(*) FROM notifications.notification_outbox WHERE status = 'Pending';
   ```
   Then trigger the Hangfire job manually from the dashboard.

### Template Not Found

Ensure templates are embedded:

```bash
dotnet build src/Modules/Booking.Modules.Notifications
```

Check embedded resources:

```bash
dotnet build src/Modules/Booking.Modules.Notifications --verbosity normal
```

### Database Migration Issues

```bash
# List pending migrations
dotnet ef migrations list --context NotificationsDbContext

# Reset and recreate
dotnet ef database drop --context NotificationsDbContext
dotnet ef database update --context NotificationsDbContext
```

---

## Performance Tuning

### Adjust Outbox Processing Frequency

In `NotificationsModule.ConfigureBackgroundJobs()`:

```csharp
// More frequent (every minute)
RecurringJob.AddOrUpdate<OutboxProcessorJob>(
    "process-notification-outbox",
    job => job.ProcessAsync(default),
    "* * * * *"); // Every minute

// Less frequent (every 5 minutes)
RecurringJob.AddOrUpdate<OutboxProcessorJob>(
    "process-notification-outbox",
    job => job.ProcessAsync(default),
    "*/5 * * * *"); // Every 5 minutes
```

### Adjust Batch Size

Modify `OutboxProcessorJob.ProcessAsync()`:

```csharp
var command = new ProcessOutboxCommand(BatchSize: 200); // Increased from 100
```

### Index Optimization

The module creates 9 indexes by default. Monitor query performance:

```sql
-- Check slow queries
SELECT * FROM pg_stat_statements
WHERE query LIKE '%notification_outbox%'
ORDER BY mean_exec_time DESC;
```

---

## Monitoring & Alerts

### Key Metrics to Monitor

1. **Pending Notifications**:

   ```sql
   SELECT COUNT(*) FROM notifications.notification_outbox WHERE status = 'Pending';
   ```

2. **Failed Notifications**:

   ```sql
   SELECT COUNT(*) FROM notifications.notification_outbox
   WHERE status = 'Failed' AND attempts >= max_attempts;
   ```

3. **Average Delivery Time**:

   ```sql
   SELECT AVG(EXTRACT(EPOCH FROM (sent_at - created_at))) as avg_seconds
   FROM notifications.notification_outbox
   WHERE status = 'Sent' AND sent_at > NOW() - INTERVAL '1 day';
   ```

4. **Retry Rate**:
   ```sql
   SELECT
       status,
       AVG(attempts) as avg_attempts,
       COUNT(*) as count
   FROM notifications.notification_outbox
   WHERE created_at > NOW() - INTERVAL '1 day'
   GROUP BY status;
   ```

---

## Next Steps

1. ✅ Module integrated and working
2. ✅ Database migrations applied
3. ✅ Background jobs running
4. 🔄 Gradually migrate existing email jobs
5. 🔄 Remove old `AwsSesEmailService` after full migration
6. 🔄 Add monitoring/alerting for failed notifications
7. 🔄 Consider adding SMS/Push notification channels

---

## Success Checklist

- [ ] Notifications module builds successfully
- [ ] Database migration applied (`notifications` schema exists)
- [ ] `notification_outbox` table created with indexes
- [ ] Hangfire recurring jobs registered
- [ ] Test email queued successfully
- [ ] Outbox processor runs every 2 minutes
- [ ] Test email delivered to inbox
- [ ] Hangfire dashboard accessible
- [ ] Logs show successful processing
- [ ] AWS SES credentials configured
- [ ] No errors in application logs

---

**Congratulations!** The Notifications module is now fully integrated. 🎉
