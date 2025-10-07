## 🔍 Type Mapping Flow

```
┌──────────────────────────────────────────────────────────────┐
│ Business Logic (CompleteWebhook, Services, etc.)             │
│ Uses: AdminAlertType & AdminAlertSeverity (Common layer)     │
└────────────────────────┬─────────────────────────────────────┘
                         │
                         ▼
┌──────────────────────────────────────────────────────────────┐
│ NotificationService                                           │
│ • Sends SignalR alert (uses enum.ToString())                 │
│ • Calls persistence with enum types                          │
└────────────────────────┬─────────────────────────────────────┘
                         │
                         ▼
┌──────────────────────────────────────────────────────────────┐
│ AdminNotificationPersistence                                  │
│ • Maps AdminAlertType → AdminNotificationType                │
│ • Maps AdminAlertSeverity → AdminNotificationSeverity        │
└────────────────────────┬─────────────────────────────────────┘
                         │
                         ▼
┌──────────────────────────────────────────────────────────────┐
│ Database (catalog.admin_notifications)                        │
│ • Stores as integers (enum values)                           │
│ • Type-safe queries possible                                 │
└──────────────────────────────────────────────────────────────┘
```
