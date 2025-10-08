# SignalR Notifications Usage Guide

## Overview

The SignalR notification system allows you to send real-time notifications to users or groups of users. This guide shows how to use the new `SignalRSendRequest` contract and related services.

## Basic Usage

### 1. Sending to a Specific User

```csharp
var request = new SignalRSendRequest
{
    UserSlug = "user123",
    Title = "New Message",
    Message = "You have received a new message",
    Type = NotificationType.System,
    Severity = NotificationSeverity.Info,
    Data = new { MessageId = 456, Sender = "admin" }
};

var result = await notificationService.SendSignalRNotificationAsync(request);
```

### 2. Sending to a Group

```csharp
var request = new SignalRSendRequest
{
    GroupName = "admins",
    Title = "System Alert",
    Message = "Server maintenance scheduled",
    Type = NotificationType.Administrative,
    Severity = NotificationSeverity.Warning,
    Data = new { MaintenanceTime = DateTime.UtcNow.AddHours(2) }
};

var result = await notificationService.SendSignalRNotificationAsync(request);
```

### 3. Using Multi-Channel Notifications

```csharp
var request = new SendNotificationRequest
{
    Recipient = "user123",
    Subject = "Payment Confirmation",
    Message = "Your payment has been processed successfully",
    Channels = new[] { NotificationChannel.Email, NotificationChannel.SignalR, NotificationChannel.InApp },
    Type = NotificationType.Payment,
    Severity = NotificationSeverity.Success,
    GroupName = null, // For user-specific SignalR
    SignalRData = new { PaymentId = 789, Amount = 99.99m }
};

var result = await notificationService.SendMultiChannelNotificationAsync(request);
```

## SignalR Hub Configuration

### In Startup.cs or Program.cs

```csharp
// Add services
builder.Services.AddNotificationsModule(builder.Configuration);

// Configure the app
app.UseNotificationsSignalR(); // This maps the hub at /api/notifications/hub
```

## Client-Side JavaScript Example

```javascript
// Connect to the hub
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/api/notifications/hub")
  .build();

// Listen for notifications
connection.on("ReceiveNotification", function (notification) {
  console.log("Received notification:", notification);

  // notification structure:
  // {
  //   Title: "string",
  //   Message: "string",
  //   Type: "string",
  //   Severity: "string",
  //   Data: object,
  //   Timestamp: "ISO date string",
  //   CorrelationId: "string"
  // }

  // Display the notification in your UI
  displayNotification(notification);
});

// Start the connection
connection
  .start()
  .then(function () {
    console.log("Connected to notifications hub");
  })
  .catch(function (err) {
    console.error("Error connecting to hub:", err);
  });

// Join a group (for admin notifications, etc.)
connection.invoke("JoinGroup", "admins");
```

## Available Methods

### ISignalRSender Methods

1. `SendToUserAsync(userSlug, notification, cancellationToken)` - Send to specific user
2. `SendToGroupAsync(groupName, notification, cancellationToken)` - Send to group
3. `SendAsync(signalRSendRequest, cancellationToken)` - Send using structured request

### INotificationService Methods

1. `SendSignalRNotificationAsync(signalRSendRequest, cancellationToken)` - Direct SignalR sending
2. `SendMultiChannelNotificationAsync(sendNotificationRequest, cancellationToken)` - Multi-channel including SignalR
3. `EnqueueMultiChannelNotificationAsync(sendNotificationRequest, cancellationToken)` - Queued multi-channel

## Hub Groups

The NotificationHub automatically manages user groups:

- **User Groups**: Users are automatically added to a group named `user_{userId}` when they connect
- **Custom Groups**: Use `JoinGroup(groupName)` and `LeaveGroup(groupName)` from the client
- **Admin Groups**: Use "admins" as the group name for administrator notifications

## Error Handling

All methods return result objects that indicate success/failure:

```csharp
var result = await signalRSender.SendAsync(request);

if (result.IsSuccess)
{
    Console.WriteLine($"Notification sent to {result.ClientCount} clients");
}
else
{
    Console.WriteLine($"Failed to send notification: {result.ErrorMessage}");
}
```

## Best Practices

1. **Validation**: Always check `request.IsValid()` before sending
2. **Error Handling**: Check result.IsSuccess and handle errors appropriately
3. **Group Management**: Use meaningful group names and manage group membership properly
4. **Data Payload**: Keep the Data payload small and JSON-serializable
5. **Connection Management**: Handle SignalR connection drops and reconnection on the client side
