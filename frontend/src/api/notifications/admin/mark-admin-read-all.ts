import { useMutation, useQueryClient, type UseMutationOptions } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { NotificationEndpoints } from '@/api/utils/notifications-endpoints';
import { notificationKeys } from '@/api/notifications/notifications-keys';

export const markAllNotificationsRead = async (): Promise<void> => {
  await api.post(NotificationEndpoints.Admin.MarkAllRead);
};

export function useMarkAllNotificationsRead(options?: UseMutationOptions<void, Error, void>) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: markAllNotificationsRead,

    meta: {
      invalidatesQuery: notificationKeys.allAdmin,
      successMessage: 'Notification marked as read successfully!',
      successAction: (data, variables, context) => {
        queryClient.setQueryData(notificationKeys.unReadCount(), () => 0);
        options?.onSuccess?.(data, variables, context);
        return;
      },
    },
    ...options,
  });
}
