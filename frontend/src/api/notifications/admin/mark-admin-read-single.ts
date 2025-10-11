import { useMutation, type UseMutationOptions } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { NotificationEndpoints } from '@/api/utils/notifications-endpoints';
import { notificationKeys } from '@/api/notifications/notifications-keys';

export const markNotificationRead = async (notificationId: number): Promise<void> => {
  await api.post(NotificationEndpoints.Admin.MarkSingleRead(notificationId));
};

export function useMarkNotificationRead(options?: UseMutationOptions<void, Error, number>) {
  return useMutation({
    mutationFn: markNotificationRead,

    meta: {
      invalidatesQuery: notificationKeys.allAdmin,
      successMessage: 'Notification marked as read successfully!',
      successAction: (data, variables, context) => options?.onSuccess?.(data, variables, context),
    },
    ...options,
  });
}
