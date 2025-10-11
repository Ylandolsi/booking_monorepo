import { queryOptions, useQuery, useQueryClient, type UseQueryOptions } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { NotificationEndpoints } from '@/api/utils/notifications-endpoints';
import { notificationKeys } from '@/api/notifications/notifications-keys';

export const getAdminUnreadNotificationsCount = async (): Promise<number> => {
  const response = await api.get<{ unreadCount: number }>(NotificationEndpoints.Admin.GetUnreadCount, {});
  return response?.unreadCount;
};

export function useGetAdminUnreadNotificationsCount(options?: Partial<UseQueryOptions<number, Error>>) {
  return useQuery(
    queryOptions({
      queryKey: notificationKeys.unReadCount(),
      queryFn: getAdminUnreadNotificationsCount,

      ...options,
    }),
  );
}
