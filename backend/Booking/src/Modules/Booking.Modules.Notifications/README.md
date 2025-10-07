# Booking.Modules.Notifications

A robust, database-backed notification module implementing the **Outbox Pattern** for transactional email delivery with automatic retry logic and background processing.

---

## Features

- ✅ **Outbox Pattern**: Transactional safety - emails are only sent if the database transaction succeeds
- ✅ **Automatic Retries**: Exponential backoff retry mechanism (configurable, default: 3 attempts)
- ✅ **Background Processing**: Asynchronous delivery using Hangfire
- ✅ **Idempotency**: Deduplication via NotificationReference to prevent duplicate sends
- ✅ **Template Engine**: Embedded HTML email templates with variable substitution
- ✅ **Priority Support**: Critical, High, Normal, Low priority levels
- ✅ **Scheduled Delivery**: Queue emails for future delivery
- ✅ **Status Tracking**: Monitor notification delivery status and history
- ✅ **AWS SES Integration**: Production-ready email delivery with Polly resilience

---

## Folder Structure

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
