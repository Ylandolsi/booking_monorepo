### Service Registration Required:

```csharp
// In your DI container setup (Program.cs or Module)

// 1. Register health checks
services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" })
    .AddCheck<HangfireHealthCheck>("hangfire", tags: new[] { "ready" })
    .AddCheck<GoogleCalendarHealthCheck>("google_calendar", tags: new[] { "ready" })
    .AddCheck<KonnectPaymentHealthCheck>("konnect_payment", tags: new[] { "ready" });

// 2. Register admin notification persistence
services.AddScoped<IAdminNotificationPersistence, AdminNotificationPersistence>();

// 3. Update NotificationService to include persistence
services.AddScoped<NotificationService>(sp =>
{
    var hubContext = sp.GetService<IHubContext<NotificationHub>>();
    var logger = sp.GetRequiredService<ILogger<NotificationService>>();
    var persistence = sp.GetService<IAdminNotificationPersistence>();
    return new NotificationService(hubContext, logger, persistence);
});
```

---

## 🎯 Final Pre-Go-Live Checklist

- [ ] Set up production monitoring dashboard
- [ ] Configure Kubernetes/Docker health probes
- [ ] Test admin SignalR notifications

---

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
