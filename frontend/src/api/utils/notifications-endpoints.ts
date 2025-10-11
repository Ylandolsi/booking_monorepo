export const NotificationEndpoints = {
  Admin: {
    Get: 'api/notifications/admin',
    GetUnreadCount : 'api/notifications/admin/count',
    MarkAllRead: 'api/notifications/admin',
    MarkSingleRead: (notificationId: number) => `api/notifications/admin/${notificationId}`,
  },
} as const;
