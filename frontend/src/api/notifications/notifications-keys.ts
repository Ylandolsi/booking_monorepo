import type { GetAdminNotificationsQuery } from '@/api/notifications/admin';

export const notificationKeys = {
  allAdmin: ['notifications', 'admin'] as const,
  queryAdmin: (query: GetAdminNotificationsQuery) => [...notificationKeys.allAdmin, query] as const,
};
