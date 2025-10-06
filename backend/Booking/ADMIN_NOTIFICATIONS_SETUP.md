# Admin Notification System & Health Checks - Production Setup Guide

## 🎯 Overview

This document describes the admin notification system and health check endpoints implemented for production monitoring and alerting.

## 📋 Table of Contents

1. [Admin Notification System](#admin-notification-system)
2. [Health Check Endpoints](#health-check-endpoints)
3. [Database Migration](#database-migration)
4. [Service Registration](#service-registration)
5. [Frontend Integration](#frontend-integration)
6. [Testing](#testing)

---

## 1. Admin Notification System

### Features

- **Real-time Notifications**: SignalR-based real-time alerts to admin users
- **Persistent Storage**: All notifications are saved to database for admin dashboard
- **Severity Levels**: Info, Warning, Error, Critical
- **Notification Types**: Integration failures, payment anomalies, session booking issues, system errors, health check failures

### Components Created

#### 1.1 AdminNotification Entity

**Location**: `Domain/Entities/AdminNotification.cs`

```csharp
public class AdminNotification : Entity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public AdminNotificationSeverity Severity { get; private set; }
    public AdminNotificationType Type { get; private set; }
    public string? RelatedEntityId { get; private set; }
    public string? RelatedEntityType { get; private set; }
    public string? Metadata { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }
}
```

#### 1.2 Enhanced NotificationService

**Location**: `Common/RealTime/NotificationService.cs`

**New Methods**:

- `SendAdminAlertAsync()` - Send critical alerts to admins
- `SendIntegrationFailureAlertAsync()` - Notify about integration failures
- `SendPaymentAnomalyAlertAsync()` - Alert about payment issues
- `SendSessionBookingAlertAsync()` - Notify about booking problems

#### 1.3 Enhanced NotificationHub

**Location**: `Common/RealTime/NotificationHub.cs`

**New Methods**:

- `JoinAdminGroup()` - Admin users join to receive alerts (requires Admin role)
- `LeaveAdminGroup()` - Leave admin notification group

#### 1.4 Admin Notification Persistence

**Location**: `Features/AdminNotifications/AdminNotificationPersistence.cs`

Implements `IAdminNotificationPersistence` to save notifications to database.

#### 1.5 API Endpoints

**Location**: `Features/AdminNotifications/`

- `GET /api/admin/notifications` - Get paginated notifications with filters
- `POST /api/admin/notifications/{id}/mark-read` - Mark notification as read
- `POST /api/admin/notifications/mark-all-read` - Mark all as read
- `DELETE /api/admin/notifications/{id}` - Delete notification

**Query Parameters**:

```
GET /api/admin/notifications?page=1&pageSize=20&unreadOnly=true&severity=Critical
```

### Usage Examples

#### Backend - Sending Admin Alerts

```csharp
// In CompleteWebhook.cs or any service
await notificationService.SendIntegrationFailureAlertAsync(
    integrationName: "Google Calendar",
    orderId: order.Id.ToString(),
    errorMessage: "Mentor not integrated with Google Calendar",
    additionalData: new { SessionId = session.Id, StoreId = store.Id }
);

// Generic admin alert
await notificationService.SendAdminAlertAsync(
    title: "Critical System Error",
    message: "Payment processing failed for order XYZ",
    severity: AdminAlertSeverity.Critical,
    metadata: new { OrderId = 123, ErrorCode = "PAYMENT_FAILED" },
    relatedEntityId: "123",
    relatedEntityType: "Order"
);
```

#### Frontend - SignalR Connection (JavaScript/TypeScript)

```typescript
import { HubConnectionBuilder } from "@microsoft/signalr";

// Connect to SignalR hub
const connection = new HubConnectionBuilder()
  .withUrl("/hubs/notifications", {
    accessTokenFactory: () => getAuthToken(),
  })
  .withAutomaticReconnect()
  .build();

// Join admin group (for admin users only)
await connection.invoke("JoinAdminGroup");

// Listen for admin alerts
connection.on("ReceiveAdminAlert", (alert) => {
  console.log("Admin Alert:", alert);
  // alert structure:
  // {
  //   id: string,
  //   type: "admin_alert",
  //   title: string,
  //   message: string,
  //   severity: "Critical" | "Error" | "Warning" | "Info",
  //   createdAt: Date,
  //   metadata: object
  // }

  // Show notification in UI
  showAdminNotification(alert);
});

await connection.start();
```

---

## 2. Health Check Endpoints

### Available Health Checks

#### 2.1 Database Health Check

**Location**: `Features/HealthChecks/HealthChecks.cs`

Checks:

- Database connectivity
- Query execution
- Returns order count as diagnostic data

#### 2.2 Hangfire Health Check

Checks:

- Hangfire server status
- Job statistics (enqueued, processing, failed)
- Alerts if failed jobs > 100

#### 2.3 Google Calendar Health Check

Checks:

- Google Calendar service configuration
- API availability

#### 2.4 Konnect Payment Health Check

Checks:

- Payment gateway configuration
- HTTP client factory setup

### Health Check Endpoints

#### 2.1 Comprehensive Health Check

```
GET /health
```

**Response**:

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "database",
      "status": "Healthy",
      "description": "Database is healthy",
      "duration": 45.2,
      "data": {
        "database": "CatalogDb",
        "status": "Connected",
        "orderCount": 1234,
        "checkedAt": "2025-10-06T10:30:00Z"
      }
    },
    {
      "name": "hangfire",
      "status": "Healthy",
      "description": "Hangfire is healthy",
      "duration": 12.5,
      "data": {
        "serverCount": 1,
        "enqueuedJobs": 5,
        "processingJobs": 2,
        "failedJobs": 3
      }
    }
  ],
  "totalDuration": 57.7,
  "timestamp": "2025-10-06T10:30:00Z"
}
```

#### 2.2 Liveness Check

```
GET /health/live
```

Simple check if service is running (no dependencies checked).

#### 2.3 Readiness Check

```
GET /health/ready
```

Checks if service is ready to accept requests (checks tagged dependencies).

---

## 3. Database Migration

### Create Migration

```bash
cd backend/Booking/src/Modules/Booking.Modules.Catalog

# Create migration for AdminNotifications table
dotnet ef migrations add AddAdminNotifications --context CatalogDbContext

# Apply migration
dotnet ef database update --context CatalogDbContext
```

### Expected Table Schema

```sql
CREATE TABLE catalog.admin_notifications (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    severity INT NOT NULL, -- 0=Info, 1=Warning, 2=Error, 3=Critical
    type INT NOT NULL, -- 0=IntegrationFailure, 1=PaymentAnomaly, etc.
    related_entity_id VARCHAR(100),
    related_entity_type VARCHAR(100),
    metadata TEXT,
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL,
    read_at TIMESTAMP
);

CREATE INDEX idx_admin_notifications_is_read ON catalog.admin_notifications(is_read);
CREATE INDEX idx_admin_notifications_severity ON catalog.admin_notifications(severity);
CREATE INDEX idx_admin_notifications_created_at ON catalog.admin_notifications(created_at DESC);
```

---

## 4. Service Registration

### Update CatalogModule.cs (or Program.cs)

```csharp
// Register health checks
services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" })
    .AddCheck<HangfireHealthCheck>("hangfire", tags: new[] { "ready" })
    .AddCheck<GoogleCalendarHealthCheck>("google_calendar", tags: new[] { "ready" })
    .AddCheck<KonnectPaymentHealthCheck>("konnect_payment", tags: new[] { "ready" });

// Register admin notification persistence
services.AddScoped<IAdminNotificationPersistence, AdminNotificationPersistence>();

// Update NotificationService registration to include persistence
services.AddScoped<NotificationService>(sp =>
{
    var hubContext = sp.GetService<IHubContext<NotificationHub>>();
    var logger = sp.GetRequiredService<ILogger<NotificationService>>();
    var persistence = sp.GetService<IAdminNotificationPersistence>();
    return new NotificationService(hubContext, logger, persistence);
});
```

### Map Endpoints in Program.cs

```csharp
// Map health check endpoints
app.MapHealthCheckEndpoints();

// SignalR hub mapping (if not already done)
app.MapHub<NotificationHub>("/hubs/notifications");
```

---

## 5. Frontend Integration

### Install SignalR Client

```bash
npm install @microsoft/signalr
```

### Admin Dashboard Example (React/TypeScript)

```typescript
// hooks/useAdminNotifications.ts
import { useState, useEffect } from "react";
import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

interface AdminAlert {
  id: string;
  type: string;
  title: string;
  message: string;
  severity: "Info" | "Warning" | "Error" | "Critical";
  createdAt: Date;
  metadata?: any;
}

export function useAdminNotifications() {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [alerts, setAlerts] = useState<AdminAlert[]>([]);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("/hubs/notifications", {
        accessTokenFactory: () => localStorage.getItem("authToken") || "",
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to SignalR");

          // Join admin group
          connection
            .invoke("JoinAdminGroup")
            .catch((err) => console.error("Failed to join admin group:", err));

          // Listen for admin alerts
          connection.on("ReceiveAdminAlert", (alert: AdminAlert) => {
            setAlerts((prev) => [alert, ...prev]);

            // Show browser notification
            if (Notification.permission === "granted") {
              new Notification(alert.title, {
                body: alert.message,
                icon: "/admin-alert-icon.png",
              });
            }
          });
        })
        .catch((err) => console.error("SignalR Connection Error:", err));
    }
  }, [connection]);

  return { alerts, connection };
}
```

### Fetch Notifications API

```typescript
// api/adminNotifications.ts
export async function getAdminNotifications(params: {
  page?: number;
  pageSize?: number;
  unreadOnly?: boolean;
  severity?: string;
}) {
  const queryParams = new URLSearchParams();
  if (params.page) queryParams.append("page", params.page.toString());
  if (params.pageSize)
    queryParams.append("pageSize", params.pageSize.toString());
  if (params.unreadOnly) queryParams.append("unreadOnly", "true");
  if (params.severity) queryParams.append("severity", params.severity);

  const response = await fetch(`/api/admin/notifications?${queryParams}`, {
    headers: {
      Authorization: `Bearer ${getToken()}`,
    },
  });

  return response.json();
}

export async function markNotificationRead(id: number) {
  await fetch(`/api/admin/notifications/${id}/mark-read`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${getToken()}`,
    },
  });
}
```
