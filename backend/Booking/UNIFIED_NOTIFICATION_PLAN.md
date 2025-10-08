# 🔄 Unified Notification System - Integration Plan

## 📋 **Phase 1: Extend the Notifications Module**

### 1.1 **Add New Notification Channels**

```csharp
// Update NotificationChannel enum - Simplified approach
public enum NotificationChannel
{
    Email = 1,
    Sms = 2,        // Future
    Push = 3,       // Future
    InApp = 4,      // For persistent dashboard notifications
    SignalR = 5     // For real-time notifications (includes admin alerts to "admins" group)
}
```

### 1.2 **Add Channel-Specific Interfaces**

```csharp
// New abstraction for SignalR notifications
public interface ISignalRSender
{
    Task SendToUserAsync(string userSlug, object notification, CancellationToken cancellationToken = default);
    Task SendToGroupAsync(string groupName, object notification, CancellationToken cancellationToken = default);
}

// New abstraction for in-app notifications (persistent storage)
public interface IInAppSender
{
    Task SaveNotificationAsync(InAppNotificationRequest request, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(string recipientId, CancellationToken cancellationToken = default);
}
```

### 1.3 **Extend SendNotificationRequest**

```csharp
public sealed record SendNotificationRequest
{
    // Existing email properties
    public required string Recipient { get; init; }
    public required NotificationChannel Channel { get; init; }
    public string? Subject { get; init; }
    public string? HtmlBody { get; init; }
    public string? TextBody { get; init; }
    public string? TemplateName { get; init; }
    public object? TemplateData { get; init; }

    // NEW: Multi-channel properties
    public string? RecipientUserId { get; init; }    // For user-specific notifications
    public string? GroupName { get; init; }           // For SignalR groups (e.g., "admins", userSlug)
    public NotificationType? Type { get; init; }      // For categorization
    public NotificationSeverity? Severity { get; init; } // For admin alerts

    // Existing properties
    public NotificationPriority Priority { get; init; } = NotificationPriority.Normal;
    public string? CorrelationId { get; init; }
    public string? NotificationReference { get; init; }
    public DateTime? ScheduledAt { get; init; }
    public string? CreatedBy { get; init; }
    public int? MaxAttempts { get; init; }
}

public enum NotificationType
{
    UserNotification,        // Personal notifications to specific users
    AdminAlert,             // Alerts to admin group (via SignalR "admins" group)
    SystemNotification,     // System-wide announcements
    IntegrationFailure,     // Integration error alerts (admin)
    PaymentAnomaly,         // Payment issues (admin)
    SessionBookingIssue,    // Booking problems (admin)
    HealthCheckFailure      // Health check alerts (admin)
}

public enum NotificationSeverity
{
    Info,
    Warning,
    Error,
    Critical
}
```

## 📋 **Phase 2: Create Channel Adapters**

### 2.1 **SignalR Adapter** (Handles both user and admin notifications)

```csharp
public class SignalRSender : ISignalRSender
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<SignalRSender> _logger;

    public async Task SendToUserAsync(string userSlug, object notification, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group(userSlug).SendAsync("ReceiveNotification", notification, cancellationToken);
        _logger.LogInformation("SignalR notification sent to user {UserSlug}", userSlug);
    }

    public async Task SendToGroupAsync(string groupName, object notification, CancellationToken cancellationToken = default)
    {
        // This handles admin alerts (groupName = "admins") and any other groups
        var eventName = groupName == "admins" ? "ReceiveAdminAlert" : "ReceiveNotification";
        await _hubContext.Clients.Group(groupName).SendAsync(eventName, notification, cancellationToken);
        _logger.LogInformation("SignalR notification sent to group {GroupName}", groupName);
    }
}
```

### 2.2 **InApp Adapter** (For persistent notifications)

```csharp
public class InAppSender : IInAppSender
{
    private readonly NotificationsDbContext _dbContext;

    public async Task SaveNotificationAsync(InAppNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var notification = new InAppNotification(
            recipientId: request.RecipientUserId,
            title: request.Subject,
            message: request.TextBody,
            type: request.Type,
            severity: request.Severity,
            metadata: request.TemplateData != null ? JsonSerializer.Serialize(request.TemplateData) : null
        );

        await _dbContext.InAppNotifications.AddAsync(notification, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
```

## 📋 **Phase 3: Update NotificationService**

### 3.1 **Multi-Channel Routing**

```csharp
public class NotificationService : INotificationService
{
    private readonly IEmailSender _emailSender;
    private readonly ISignalRSender _signalRSender;
    private readonly IInAppSender _inAppSender;
    // ... existing dependencies

    public async Task<SendNotificationResult> EnqueueNotificationAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        // Route based on channel
        return request.Channel switch
        {
            NotificationChannel.Email => await EnqueueEmailAsync(request, cancellationToken),
            NotificationChannel.SignalR => await EnqueueSignalRAsync(request, cancellationToken),
            NotificationChannel.InApp => await EnqueueInAppAsync(request, cancellationToken),
            _ => throw new NotSupportedException($"Channel {request.Channel} not supported")
        };
    }

    // NEW: Multi-channel notification (send to multiple channels at once)
    public async Task<MultiChannelResult> SendMultiChannelAsync(
        MultiChannelRequest request,
        CancellationToken cancellationToken = default)
    {
        var results = new List<SendNotificationResult>();

        foreach (var channel in request.Channels)
        {
            var channelRequest = request.ToChannelRequest(channel);
            var result = await EnqueueNotificationAsync(channelRequest, cancellationToken);
            results.Add(result);
        }

        return new MultiChannelResult(results);
    }
}
```

## 📋 **Phase 4: Migration Strategy**

### 4.1 **Create Compatibility Layer**

```csharp
// Wrapper to maintain backward compatibility
public class LegacyNotificationServiceAdapter
{
    private readonly INotificationService _newNotificationService;

    // Map old SendAdminAlertAsync to new system
    public async Task SendAdminAlertAsync(
        string title,
        string message,
        AdminAlertSeverity severity = AdminAlertSeverity.Error,
        AdminAlertType type = AdminAlertType.SystemError,
        // ... other parameters
        CancellationToken cancellationToken = default)
    {
        // Admin alerts = SignalR to "admins" group + InApp for persistence
        await _newNotificationService.SendMultiChannelAsync(new MultiChannelRequest
        {
            Channels = [NotificationChannel.SignalR, NotificationChannel.InApp],
            GroupName = "admins",  // SignalR will route to "admins" group
            RecipientUserId = "admins", // InApp will store for admin dashboard
            Subject = title,
            TextBody = message,
            Type = MapAdminAlertType(type),
            Severity = MapAdminAlertSeverity(severity),
            TemplateData = metadata,
            Priority = severity == AdminAlertSeverity.Critical
                ? NotificationPriority.Critical
                : NotificationPriority.High
        }, cancellationToken);
    }

    // Map old SendNotificationAsync to new system
    public async Task SendNotificationAsync(string userSlug, NotificationDto notification)
    {
        await _newNotificationService.EnqueueNotificationAsync(new SendNotificationRequest
        {
            Channel = NotificationChannel.SignalR,
            GroupName = userSlug,  // User-specific SignalR group
            Subject = notification.Title,
            TextBody = notification.Message,
            Type = NotificationType.UserNotification,
            TemplateData = notification.Data
        });
    }
}    // Map old SendNotificationAsync to new system
    public async Task SendNotificationAsync(string userSlug, NotificationDto notification)
    {
        await _newNotificationService.EnqueueNotificationAsync(new SendNotificationRequest
        {
            Channel = NotificationChannel.SignalR,
            RecipientUserId = userSlug,
            Subject = notification.Title,
            TextBody = notification.Message,
            Type = NotificationType.UserNotification,
            TemplateData = notification.Data
        });
    }
}
```

### 4.2 **Database Migration**

```csharp
// Move AdminNotification from Catalog to Notifications module
// Create new InAppNotification entity that can handle both admin and user notifications

public class InAppNotification : Entity
{
    public Guid Id { get; private set; }
    public string RecipientId { get; private set; }  // User ID or "admins" for admin notifications
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationSeverity Severity { get; private set; }
    public string? Metadata { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // ... methods
}
```

## 📋 **Phase 5: Enhanced Features**

### 5.1 **Notification Preferences**

```csharp
public class NotificationPreferences : Entity
{
    public string UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public List<NotificationChannel> EnabledChannels { get; private set; }
    public bool IsEnabled { get; private set; }

    // User can choose: Email only, SignalR only, Both, None
}
```

### 5.2 **Template Support for All Channels**

```csharp
// Templates can now render for different channels
public interface ITemplateEngine
{
    Task<ChannelContent> RenderForChannelAsync(
        string templateName,
        NotificationChannel channel,
        object? data,
        CancellationToken cancellationToken = default);
}

public record ChannelContent(
    string? Subject,      // For email/in-app
    string? HtmlBody,     // For email
    string? TextBody,     // For email/in-app/SignalR
    object? SignalRData   // For SignalR
);
```

## 🎯 **Benefits of This Simplified Approach**

1. **✅ Unified API**: One service for all notification types
2. **✅ Backward Compatibility**: Existing code continues to work
3. **✅ Multi-Channel**: Send same notification via multiple channels
4. **✅ Reliability**: Outbox pattern for all channels
5. **✅ Extensible**: Easy to add SMS, Push, etc.
6. **✅ User Preferences**: Users control how they receive notifications
7. **✅ Template Support**: Consistent templating across channels
8. **✅ Status Tracking**: Track delivery across all channels
9. **✅ Cleaner Architecture**: Admin alerts are just SignalR to "admins" group

## 📝 **Simplified Usage Examples**

```csharp
// ✅ Admin Alert (SignalR to "admins" group + InApp persistence)
await notificationService.SendMultiChannelAsync(new MultiChannelRequest
{
    Channels = [NotificationChannel.SignalR, NotificationChannel.InApp],
    GroupName = "admins",           // SignalR routes to "admins" group
    RecipientUserId = "admins",     // InApp stores for admin dashboard
    Subject = "Payment Integration Failed",
    TemplateName = "AdminAlert",
    Type = NotificationType.AdminAlert,
    Severity = NotificationSeverity.Critical
});

// ✅ User Notification (SignalR to user group + Email fallback)
await notificationService.SendMultiChannelAsync(new MultiChannelRequest
{
    Channels = [NotificationChannel.SignalR, NotificationChannel.Email],
    GroupName = "user-john-doe",    // SignalR routes to user's group
    Recipients = ["john@example.com"], // Email fallback
    Subject = "Your booking is confirmed",
    TemplateName = "BookingConfirmation"
});

// ✅ Pure Email (existing functionality)
await notificationService.EnqueueEmailAsync(new SendEmailRequest
{
    Recipient = "user@example.com",
    Subject = "Password Reset",
    TemplateName = "PasswordReset",
    TemplateData = new { ResetLink = "..." }
});
```

## 🔄 **Key Insight: Admin = SignalR Group**

You're absolutely right! This simplification makes the architecture much cleaner:

- **SignalR Channel**: Handles real-time delivery to users AND admins
  - User notifications: `GroupName = userSlug`
  - Admin alerts: `GroupName = "admins"`
- **InApp Channel**: Handles persistent storage for dashboard
- **Email Channel**: Handles reliable email delivery

**No separate "Admin Channel" needed!** Admin notifications are just SignalR notifications sent to the "admins" group, with optional InApp persistence for the dashboard.

## 📝 **Migration Timeline**

### **Week 1**: Extend NotificationChannel enum and add SignalR/InApp interfaces

### **Week 2**: Implement SignalR and InApp adapters

### **Week 3**: Update NotificationService with multi-channel support

### **Week 4**: Create compatibility layer and database migration

### **Week 5**: Migrate existing usages and test

### **Week 6**: Add enhanced features (preferences, multi-channel templates)

This approach maintains clean separation of concerns while providing a unified, extensible notification system that recognizes admin alerts are simply group-based SignalR notifications.
