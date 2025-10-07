# Notifications Module - Migration Guide

## Overview

The old `AwsSesEmailService` in `Booking.Common.Email` has been replaced with the new `INotificationService` in the `Booking.Modules.Notifications` module.

## Why Migrate?

The new notification system provides:

- **Transactional Safety**: Outbox pattern ensures emails aren't lost if the transaction fails
- **Retry Logic**: Automatic exponential backoff retry mechanism
- **Background Processing**: Asynchronous delivery with Hangfire
- **Idempotency**: Deduplication via NotificationReference
- **Monitoring**: Track notification status and history
- **Scalability**: Batch processing and configurable priorities

## Migration Path

### Step 1: Update Module Dependencies

Add reference to the Notifications abstractions package:

```xml
<ProjectReference Include="..\..\Modules\Booking.Modules.Notifications.Abstractions\Booking.Modules.Notifications.Abstractions.csproj" />
```

### Step 2: Update DI Registration

**Before (in UsersModule.cs or CatalogModule.cs):**

```csharp
services.AddScoped<AwsSesEmailService>();
```

**After:**

```csharp
// Remove the old registration
// services.AddScoped<AwsSesEmailService>(); // REMOVE THIS

// INotificationService is automatically available from NotificationsModule
```

### Step 3: Update Background Jobs

**Before (VerificationEmailForRegistrationJob.cs):**

```csharp
using Booking.Common.Email;

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
        var (subject, body) = await _emailTemplateProvider.GetTemplateAsync(
            TemplatesNames.VerificationEmailForRegistration,
            new { VERIFICATION_LINK = verificationLink });

        await _emailService.SendEmailAsync(userEmail, subject, body, cancellationToken);
    }
}
```

**After:**

```csharp
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Abstractions.Contracts;

public class VerificationEmailForRegistrationJob
{
    private readonly INotificationService _notificationService;

    public VerificationEmailForRegistrationJob(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task SendAsync(string userEmail, string verificationLink)
    {
        var request = new SendEmailRequest
        {
            Recipient = userEmail,
            Subject = "Verify Your Email", // Will be overridden by template
            TemplateName = "VerificationEmailForRegistration",
            TemplateData = new
            {
                VERIFICATION_LINK = verificationLink,
                APP_NAME = "Your App Name",
                SUPPORT_LINK = "https://yourapp.com/support",
                SECURITY_LINK = "https://yourapp.com/security"
            },
            NotificationReference = $"verification-{userEmail}-{DateTime.UtcNow:yyyyMMdd}",
            Priority = NotificationPriority.High
        };

        var result = await _notificationService.EnqueueEmailAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new Exception($"Failed to queue verification email: {result.ErrorMessage}");
        }
    }
}
```

### Step 4: Update SendingPasswordResetToken

Similar pattern for password reset:

```csharp
var request = new SendEmailRequest
{
    Recipient = userEmail,
    Subject = "Reset Your Password",
    TemplateName = "PasswordResetEmail",
    TemplateData = new
    {
        RESET_LINK = resetLink,
        APP_NAME = "Your App Name",
        SUPPORT_LINK = "https://yourapp.com/support",
        SECURITY_LINK = "https://yourapp.com/security"
    },
    NotificationReference = $"password-reset-{userEmail}-{DateTime.UtcNow:yyyyMMdd}",
    Priority = NotificationPriority.Critical
};

await _notificationService.EnqueueEmailAsync(request, cancellationToken);
```

## Template Variable Mapping

Templates now use `{{VARIABLE_NAME}}` syntax:

| Old Property     | New Variable            |
| ---------------- | ----------------------- |
| VerificationLink | `{{VERIFICATION_LINK}}` |
| ResetLink        | `{{RESET_LINK}}`        |
| AppName          | `{{APP_NAME}}`          |
| SupportLink      | `{{SUPPORT_LINK}}`      |
| SecurityLink     | `{{SECURITY_LINK}}`     |

The template engine automatically converts UPPER_CASE to PascalCase when matching properties.

## Benefits of New Approach

### 1. Transactional Safety

```csharp
// Old: Email sent immediately, lost if transaction rolls back
await dbContext.Users.AddAsync(user);
await _emailService.SendEmailAsync(user.Email, subject, body);
await dbContext.SaveChangesAsync(); // If this fails, email was already sent!

// New: Email queued within transaction
await dbContext.Users.AddAsync(user);
await _notificationService.EnqueueEmailAsync(request);
await dbContext.SaveChangesAsync(); // Email only sent if this succeeds
```

### 2. Automatic Retries

The outbox processor automatically retries failed emails with exponential backoff:

- Attempt 1: Immediate
- Attempt 2: 1 minute later
- Attempt 3: 2 minutes later
- Max: 1 hour between retries

### 3. Monitoring

```csharp
var result = await _notificationService.EnqueueEmailAsync(request);
var notificationId = result.NotificationId;

// Later: Check status
var status = await _notificationService.GetNotificationStatusAsync(notificationId);
Console.WriteLine($"Status: {status.Status}, Attempts: {status.Attempts}/{status.MaxAttempts}");
```

### 4. Idempotency

```csharp
// Prevents duplicate emails
var request = new SendEmailRequest
{
    ...
    NotificationReference = $"order-confirmation-{orderId}" // Same ref = deduplicated
};
```

## Cleanup (Optional)

After migration is complete, you can:

1. Mark `AwsSesEmailService` as obsolete:

```csharp
[Obsolete("Use INotificationService.EnqueueEmailAsync instead")]
public sealed class AwsSesEmailService { ... }
```

2. Remove `EmailTemplateProvider` from Booking.Common (templates now in Notifications module)

3. Remove email-related configuration from modules (centralized in Notifications module)

## Configuration

Update `appsettings.json`:

```json
{
  "Email": {
    "AwsSes": {
      "SenderEmail": "noreply@yourapp.com",
      "MaxRetryAttempts": 3,
      "RetryDelayMilliseconds": 1000
    }
  }
}
```

## Testing

```csharp
// Integration test example
[Fact]
public async Task Should_Queue_Verification_Email()
{
    // Arrange
    var request = new SendEmailRequest { ... };

    // Act
    var result = await _notificationService.EnqueueEmailAsync(request);

    // Assert
    Assert.True(result.IsSuccess);
    var notification = await _dbContext.NotificationOutbox
        .FirstAsync(n => n.Id == result.NotificationId);
    Assert.Equal(NotificationStatus.Pending, notification.Status);
}
```

## Support

For questions or issues:

- Check the notification status in the database: `SELECT * FROM notifications.notification_outbox;`
- View Hangfire dashboard for background job status
- Check logs for outbox processor execution
