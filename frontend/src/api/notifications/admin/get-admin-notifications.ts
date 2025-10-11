import { queryOptions, useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { NotificationEndpoints } from '@/api/utils/notifications-endpoints';
import { type AdminNotificationDto, type GetAdminNotificationsQuery } from './types';
import { type PaginatedResult } from '@/types/paginated-result';
import { notificationKeys } from '@/api/notifications/notifications-keys';


export const getAdminNotifications = async (query: GetAdminNotificationsQuery): Promise<PaginatedResult<AdminNotificationDto>> => {
  const response = await api.get<PaginatedResult<AdminNotificationDto>>(NotificationEndpoints.Admin.Get, {
    params: query,
  });
  return response;
};

export function useGetAdminNotifications(
  query: GetAdminNotificationsQuery,
  options?: Partial<UseQueryOptions<PaginatedResult<AdminNotificationDto>, Error>>,
) {
  return useQuery(
    queryOptions({
      queryKey: notificationKeys.queryAdmin(query),
      queryFn: () => getAdminNotifications(query),
      ...options,
    }),
  );
}
