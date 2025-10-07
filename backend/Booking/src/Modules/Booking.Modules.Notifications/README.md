# Booking.Modules.Notifications

A robust, database-backed notification module implementing the **Outbox Pattern** for transactional email delivery with automatic retry logic and background processing.

---

## 🎯 Features

- ✅ **Outbox Pattern**: Transactional safety - emails are only sent if the database transaction succeeds
- ✅ **Automatic Retries**: Exponential backoff retry mechanism (configurable, default: 3 attempts)
- ✅ **Background Processing**: Asynchronous delivery using Hangfire
- ✅ **Idempotency**: Deduplication via NotificationReference to prevent duplicate sends
- ✅ **Template Engine**: Embedded HTML email templates with variable substitution
- ✅ **Priority Support**: Critical, High, Normal, Low priority levels
- ✅ **Scheduled Delivery**: Queue emails for future delivery
- ✅ **Status Tracking**: Monitor notification delivery status and history
- ✅ **AWS SES Integration**: Production-ready email delivery with Polly resilience
- ✅ **Clean Architecture**: Modular design with clear separation of concerns

---

## 📁 Architecture

```
Booking.Modules.Notifications/
├── Abstractions/                          # Public contracts (no dependencies)
│   ├── INotificationService.cs            # High-level service interface
│   ├── IEmailSender.cs                    # Provider adapter interface
│   └── Contracts/                         # DTOs and enums
│       ├── SendEmailRequest.cs
│       ├── SendNotificationResult.cs
│       ├── NotificationStatusResponse.cs
│       ├── NotificationChannel.cs         # Email, SMS, Push, InApp
│       ├── NotificationStatus.cs          # Pending, Processing, Sent, Failed, Cancelled
│       └── NotificationPriority.cs        # Critical, High, Normal, Low
│
├── Domain/
│   └── Entities/
│       └── NotificationOutbox.cs          # Rich domain entity with business logic
│
├── Persistence/
│   ├── NotificationsDbContext.cs          # EF Core DbContext
│   ├── IUnitOfWork.cs & UnitOfWork.cs     # Transaction management
│   ├── Configurations/                    # EF Core entity configurations
│   └── Migrations/                        # Database migrations
│
├── Infrastructure/
│   ├── Adapters/
│   │   └── AwsSes/
│   │       ├── AwsSesEmailSender.cs       # AWS SES implementation
│   │       └── AwsSesOptions.cs
│   └── TemplateEngine/
│       ├── ITemplateEngine.cs
│       ├── EmbeddedTemplateEngine.cs      # Template rendering with caching
│       ├── TemplateNames.cs
│       └── Templates/                     # Embedded HTML templates
│           ├── VerificationEmailForRegistration.html
│           └── PasswordResetEmail.html
│
├── Features/                              # CQRS commands & handlers
│   └── Outbox/
│       ├── Enqueue/
│       │   └── EnqueueNotificationCommand.cs
│       └── Process/
│           └── ProcessOutboxCommand.cs
│
├── BackgroundJobs/
│   ├── OutboxProcessor/
│   │   └── OutboxProcessorJob.cs          # Processes pending notifications
│   └── Cleanup/
│       └── NotificationCleanupJob.cs      # Cleans up old notifications
│
├── Services/
│   └── NotificationService.cs             # High-level service implementation
│
└── NotificationsModule.cs                 # DI registration & configuration
```

---

## 🗄️ Database Schema

**Schema:** `notifications`

**Table:** `notification_outbox`

### Columns

- `id` (uuid, PK)
- `notification_reference` (varchar(255), unique, nullable) - for idempotency
- `recipient` (varchar(500), required)
- `channel` (varchar(50)) - Email, SMS, Push, InApp
- `subject` (text, nullable)
- `payload` (jsonb) - Template data
- `template_name` (varchar(100), nullable)
- `priority` (varchar(50)) - Critical, High, Normal, Low
- `status` (varchar(50)) - Pending, Processing, Sent, Failed, Cancelled
- `attempts` (int, default: 0)
- `max_attempts` (int, default: 3)
- `last_error` (text, nullable)
- `last_attempt_at` (timestamp, nullable)
- `sent_at` (timestamp, nullable)
- `scheduled_at` (timestamp, nullable)
- `provider_message_id` (varchar(500), nullable)
- `correlation_id` (varchar(255), nullable)
- `created_by` (varchar(255), nullable)
- `created_at` (timestamp, required)
- `updated_at` (timestamp, nullable)

### Indexes

1. `notification_reference` (unique)
2. `recipient`
3. `correlation_id`
4. `status + scheduled_at` (composite, for outbox processing)
5. `created_at`
6. `status + attempts + max_attempts` (composite, for retry candidates)
7. `status + sent_at` (composite, for cleanup)
8. `status + last_attempt_at` (composite, for failed notifications)
9. `status + updated_at` (composite, for cancelled notifications)

---

## 🚀 Usage

### Basic Email Sending (Recommended)

```csharp
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Abstractions.Contracts;

public class UserService
{
    private readonly INotificationService _notificationService;

    public UserService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task RegisterUserAsync(User user)
    {
        // Save user to database
        await _dbContext.Users.AddAsync(user);

        // Queue verification email (within same transaction)
        var emailRequest = new SendEmailRequest
        {
            Recipient = user.Email,
            Subject = "Welcome! Verify your email",
            TemplateName = "VerificationEmailForRegistration",
            TemplateData = new
            {
                VERIFICATION_LINK = $"https://app.com/verify?token={user.VerificationToken}",
                APP_NAME = "Booking App",
                SUPPORT_LINK = "https://app.com/support",
                SECURITY_LINK = "https://app.com/security"
            },
            NotificationReference = $"verification-{user.Email}",
            Priority = NotificationPriority.High
        };

        var result = await _notificationService.EnqueueEmailAsync(emailRequest);

        // Commit transaction - email only sent if this succeeds!
        await _dbContext.SaveChangesAsync();

        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to queue verification email: {Error}", result.ErrorMessage);
        }
    }
}
```

### Track Notification Status

```csharp
var result = await _notificationService.EnqueueEmailAsync(request);

// Later, check status
var status = await _notificationService.GetNotificationStatusAsync(result.NotificationId);

if (status != null)
{
    Console.WriteLine($"Status: {status.Status}");
    Console.WriteLine($"Attempts: {status.Attempts}/{status.MaxAttempts}");
    Console.WriteLine($"Last Error: {status.LastError}");
    Console.WriteLine($"Sent At: {status.SentAt}");
}
```

### Cancel Pending Notification

```csharp
var cancelled = await _notificationService.CancelNotificationAsync(notificationId);
```

### Immediate Send (Use Sparingly)

```csharp
// Bypasses outbox pattern - only for critical, time-sensitive notifications
var result = await _notificationService.SendEmailAsync(request);
```

---

## 🎨 Email Templates

Templates are stored as embedded resources and use `{{VARIABLE_NAME}}` syntax:

**Example: VerificationEmailForRegistration.html**

```html
Subject: Verify Your Email Address

<!DOCTYPE html>
<html>
  <body>
    <h1>Welcome to {{APP_NAME}}!</h1>
    <p>Please verify your email address by clicking the button below:</p>
    <a
      href="{{VERIFICATION_LINK}}"
      style="background:#4CAF50;color:white;padding:12px 24px;"
    >
      Verify Email
    </a>
    <p>Need help? <a href="{{SUPPORT_LINK}}">Contact Support</a></p>
  </body>
</html>
```

### Template Variable Mapping

The template engine automatically converts:

- `{{VERIFICATION_LINK}}` → `VerificationLink` property
- `{{APP_NAME}}` → `AppName` property
- `{{RESET_LINK}}` → `ResetLink` property

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Database=booking;Username=postgres;Password=password"
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

### Background Jobs

Configured in `NotificationsModule.ConfigureBackgroundJobs()`:

**Outbox Processor:**

- Frequency: Every 2 minutes
- Batch Size: 100 notifications per run
- Cron: `*/2 * * * *`

**Cleanup Job:**

- Frequency: Daily at 2:00 AM
- Retention: 30 days
- Cron: `0 2 * * *`

---

## 🔧 Retry Logic

### Exponential Backoff

Failed notifications are automatically retried with exponential backoff:

| Attempt | Delay     |
| ------- | --------- |
| 1       | Immediate |
| 2       | 1 minute  |
| 3       | 2 minutes |
| 4       | 4 minutes |
| 5       | 8 minutes |
| Max     | 1 hour    |

After `max_attempts` (default: 3), the notification status is set to `Failed`.

### Polly Resilience

AWS SES adapter uses Polly for transient fault handling:

- Retries: 3 attempts
- Backoff: Exponential
- Exception Types: `AmazonSimpleEmailServiceException`

---

## 📊 Monitoring

### Hangfire Dashboard

Access at `/hangfire`:

- View recurring jobs status
- Monitor outbox processor execution
- Check for failed jobs
- Manually trigger jobs

### Database Queries

```sql
-- Pending notifications
SELECT * FROM notifications.notification_outbox
WHERE status = 'Pending'
ORDER BY priority, scheduled_at;

-- Failed notifications
SELECT id, recipient, status, attempts, last_error
FROM notifications.notification_outbox
WHERE status = 'Failed';

-- Delivery metrics (last 24 hours)
SELECT
    status,
    COUNT(*) as count,
    AVG(attempts) as avg_attempts,
    AVG(EXTRACT(EPOCH FROM (sent_at - created_at))) as avg_delivery_seconds
FROM notifications.notification_outbox
WHERE created_at > NOW() - INTERVAL '1 day'
GROUP BY status;
```

---

## 🧪 Testing

### Unit Tests

```csharp
[Fact]
public async Task EnqueueEmailAsync_Should_Create_Pending_Notification()
{
    // Arrange
    var request = new SendEmailRequest
    {
        Recipient = "test@example.com",
        Subject = "Test",
        TemplateName = "VerificationEmailForRegistration",
        TemplateData = new { VERIFICATION_LINK = "https://test.com" }
    };

    // Act
    var result = await _notificationService.EnqueueEmailAsync(request);

    // Assert
    Assert.True(result.IsSuccess);
    var notification = await _dbContext.NotificationOutbox.FindAsync(result.NotificationId);
    Assert.Equal(NotificationStatus.Pending, notification.Status);
}
```

### Integration Tests

```csharp
[Fact]
public async Task Outbox_Processor_Should_Send_Pending_Emails()
{
    // Arrange
    await EnqueueTestEmail();

    // Act
    var command = new ProcessOutboxCommand(BatchSize: 100);
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(1, result.Value.SuccessCount);

    var notification = await _dbContext.NotificationOutbox.FirstAsync();
    Assert.Equal(NotificationStatus.Sent, notification.Status);
    Assert.NotNull(notification.SentAt);
}
```

---

## 🔄 Migration from Old Email Service

See [MIGRATION_GUIDE.md](./MIGRATION_GUIDE.md) for detailed migration instructions from `Booking.Common.Email.AwsSesEmailService`.

---

## 📚 Documentation

- **[INTEGRATION_GUIDE.md](./INTEGRATION_GUIDE.md)**: Step-by-step integration guide
- **[MIGRATION_GUIDE.md](./MIGRATION_GUIDE.md)**: Migrate from old email service
- **Swagger/OpenAPI**: API endpoints (if exposed)

---

## 🛠️ Development

### Add New Template

1. Create HTML file in `Infrastructure/TemplateEngine/Templates/`
2. Add constant to `TemplateNames.cs`
3. Configure in `.csproj`:
   ```xml
   <EmbeddedResource Include="Infrastructure\TemplateEngine\Templates\**\*.html" />
   ```
4. Use in code:
   ```csharp
   TemplateName = TemplateNames.YourNewTemplate
   ```

### Add New Email Provider

1. Create adapter in `Infrastructure/Adapters/YourProvider/`
2. Implement `IEmailSender`
3. Register in `NotificationsModule.AddAdapters()`

---

## 📦 Dependencies

- **EF Core 9.0.6**: Database access
- **Npgsql 9.0.4**: PostgreSQL provider
- **Hangfire 1.8.20**: Background job processing
- **Polly 8.5.0**: Resilience and retry policies
- **AWS SDK - SimpleEmail 4.0.0.12**: AWS SES integration
- **FluentValidation 12.0.0**: Input validation
- **Newtonsoft.Json 13.0.3**: JSON serialization

---

## 📝 License

Part of the Booking application. See main repository for license information.

---

## 🤝 Contributing

1. Follow existing architectural patterns (CQRS, clean architecture)
2. Add unit tests for new features
3. Update documentation
4. Follow C# coding conventions

---

## 🎉 Success!

The Notifications module is production-ready and provides:

- **Reliability**: Outbox pattern ensures no lost emails
- **Observability**: Full tracking and monitoring
- **Scalability**: Batch processing and background jobs
- **Maintainability**: Clean architecture and separation of concerns

For questions or issues, check the troubleshooting section in the [INTEGRATION_GUIDE.md](./INTEGRATION_GUIDE.md).
