export const NotificationEndpoints = {
  Admin: {
    Get: 'api/notifications/admin',
    MarkAllRead: 'api/notifications/admin',
    MarkSingleRead: (notificationId: number) => `api/notifications/admin/${notificationId}`,
  },
} as const;
