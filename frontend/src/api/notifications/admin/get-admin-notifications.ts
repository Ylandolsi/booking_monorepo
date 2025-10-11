import { queryOptions, useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { NotificationEndpoints } from '@/api/utils/notifications-endpoints';
import { adminNotificationDtoSchema, type AdminNotificationDto, type GetAdminNotificationsQuery } from './types';
import { paginatedResultSchema, type PaginatedResult } from '@/types/paginated-result';
import { notificationKeys } from '@/api/notifications/notifications-keys';

const paginatedAdminNotificationsSchema = paginatedResultSchema(adminNotificationDtoSchema);

export const getAdminNotifications = async (query: GetAdminNotificationsQuery): Promise<PaginatedResult<AdminNotificationDto>> => {
  const response = await api.get(NotificationEndpoints.Admin.Get, {
    params: query,
  });
  return paginatedAdminNotificationsSchema.parse(response);
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
