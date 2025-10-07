# 📧 How to Use the Notifications Module - Simple Example

## ❌ Before (Complicated - Old Way)

You had to:

1. Create a separate background job class (`SendingPasswordResetToken.cs`)
2. Inject `AwsSesEmailService`, `EmailTemplateProvider`, `IBackgroundJobClient`
3. Manually handle template loading and replacement
4. Manually enqueue Hangfire jobs
5. Handle retries manually

```csharp
// OLD: ForgotPasswordCommandHandler.cs
public ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    IBackgroundJobClient backgroundJobClient, // ❌ Hangfire dependency
    ...)
{
    // Enqueue a background job manually
    backgroundJobClient.Enqueue<SendingPasswordResetToken>(
        job => job.SendAsync(email, resetUrl, null));
}

// OLD: SendingPasswordResetToken.cs (74 lines!)
public class SendingPasswordResetToken
{
    private readonly AwsSesEmailService _emailService;
    private readonly EmailTemplateProvider _emailTemplateProvider;
    // ... lots of boilerplate

    public async Task SendAsync(string userEmail, string resetUrl, PerformContext? context)
    {
        // Manual template loading
        var (subject, body) = await _emailTemplateProvider.GetTemplateAsync(...);

        // Manual string replacement
        body = body.Replace("{{RESET_LINK}}", resetUrl)
                   .Replace("{{APP_NAME}}", appName)
                   .Replace("{{SUPPORT_LINK}}", supportLink);

        // Manual sending
        await _emailService.SendEmailAsync(userEmail, subject, body, ...);

        // Manual error handling, retry logic, etc.
    }
}
```

---

## ✅ After (Simple - New Way)

Just inject `INotificationService` and call **one method**! 🎉

```csharp
// NEW: ForgotPasswordCommandHandler.cs
public ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    INotificationService notificationService, // ✅ Single dependency
    ...)
{
    // Just call one method - everything else is automatic!
    await notificationService.EnqueueEmailAsync(new SendEmailRequest
    {
        Recipient = command.Email,
        Subject = "Reset Your Password",
        TemplateName = "PasswordResetEmail", // ✅ Automatic template loading
        TemplateData = new // ✅ Automatic variable replacement
        {
            RESET_LINK = resetUrl,
            APP_NAME = frontendApplicationOptions.AppName,
            SUPPORT_LINK = frontendApplicationOptions.SupportLink,
            SECURITY_LINK = frontendApplicationOptions.SecurityLink
        },
        Priority = NotificationPriority.High, // ✅ Built-in priority
        CorrelationId = $"pwd-reset-{user.Id}" // ✅ Built-in tracking
    }, cancellationToken);
}
```

**No separate background job class needed!** 🚀

---

## 🎁 What You Get Automatically

When you call `EnqueueEmailAsync`, the Notifications module automatically:

✅ **Saves to database** (Outbox Pattern for transactional safety)  
✅ **Queues for background processing** (Hangfire integration)  
✅ **Loads and renders template** (from embedded resources)  
✅ **Replaces variables** ({{RESET_LINK}} → actual URL)  
✅ **Sends via AWS SES** (with Polly retry policies)  
✅ **Retries on failure** (exponential backoff, max 3 attempts)  
✅ **Tracks status** (can query notification status later)  
✅ **Prevents duplicates** (via CorrelationId)  
✅ **Handles errors** (automatic logging and cleanup)

---

## 📋 Common Use Cases

### 1. **Password Reset Email** (High Priority)

```csharp
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = user.Email,
    Subject = "Reset Your Password",
    TemplateName = "PasswordResetEmail",
    TemplateData = new { RESET_LINK = resetUrl },
    Priority = NotificationPriority.High // Sent quickly!
}, cancellationToken);
```

### 2. **Welcome Email** (Normal Priority)

```csharp
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = newUser.Email,
    Subject = "Welcome to Our Platform!",
    TemplateName = "WelcomeEmail",
    TemplateData = new { USER_NAME = newUser.FirstName },
    Priority = NotificationPriority.Normal
}, cancellationToken);
```

### 3. **Booking Confirmation** (Critical Priority)

```csharp
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = booking.UserEmail,
    Subject = "Booking Confirmed",
    TemplateName = "BookingConfirmation",
    TemplateData = new
    {
        BOOKING_ID = booking.Id,
        SESSION_DATE = booking.StartTime.ToString("MMMM dd, yyyy"),
        MENTOR_NAME = booking.MentorName
    },
    Priority = NotificationPriority.Critical, // Highest priority!
    NotificationReference = $"booking-{booking.Id}" // Prevents duplicates
}, cancellationToken);
```

### 4. **Scheduled Email** (Send Later)

```csharp
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = user.Email,
    Subject = "Your Session Starts Tomorrow",
    TemplateName = "SessionReminder",
    TemplateData = new { SESSION_TIME = session.StartTime },
    ScheduledAt = session.StartTime.AddHours(-24) // Send 24h before
}, cancellationToken);
```

### 5. **Simple HTML Email** (No Template)

```csharp
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = admin.Email,
    Subject = "System Alert",
    HtmlBody = "<h1>Alert!</h1><p>Something needs attention.</p>",
    Priority = NotificationPriority.Critical
}, cancellationToken);
```

---

## 🔍 Track Notification Status

```csharp
// After sending
var result = await notificationService.EnqueueEmailAsync(request, cancellationToken);
var notificationId = result.NotificationId; // Save this!

// Later, check status
var status = await notificationService.GetNotificationStatusAsync(notificationId);
if (status?.Status == NotificationStatus.Sent)
{
    logger.LogInformation("Email sent successfully at {SentAt}", status.SentAt);
}
else if (status?.Status == NotificationStatus.Failed)
{
    logger.LogError("Email failed: {Error}", status.LastError);
}
```

---

## 🎯 Key Differences Summary

| **Old Way**                  | **New Way**                     |
| ---------------------------- | ------------------------------- |
| 74-line background job class | 1 method call                   |
| Manual Hangfire enqueue      | Automatic background processing |
| Manual template loading      | Automatic template engine       |
| Manual string replacement    | Automatic variable substitution |
| Manual retry logic           | Automatic exponential backoff   |
| No status tracking           | Built-in status tracking        |
| No deduplication             | Built-in idempotency            |
| Coupled to Hangfire          | Decoupled via interface         |

---

## 🚀 That's It!

You went from **74 lines of boilerplate** to **one clean method call**.

The Notifications module handles all the complexity behind the scenes:

- Database persistence
- Background processing
- Template rendering
- Email delivery
- Retries
- Error handling
- Status tracking

**Just inject `INotificationService` and start sending emails!** 🎉
