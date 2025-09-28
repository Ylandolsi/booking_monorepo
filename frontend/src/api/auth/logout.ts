import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toast } from 'sonner';
import { api } from '@/lib/api/api-client';
import * as Endpoints from '@/api/utils/auth-endpoints';
import { authQueryKeys } from '@/api/auth';

const logout = async (): Promise<void> => await api.post(Endpoints.Logout);

export const useLogout = ({
  onSuccess,
}: {
  onSuccess?: () => void;
} = {}) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: logout,
    onSuccess: () => {
      queryClient.removeQueries({ queryKey: authQueryKeys.currentUser() });
      queryClient.clear(); // clear all

      toast.success('Logged out successfully!');
      onSuccess?.();
    },
    onError: (error) => {
      console.error('Logout failed:', error);
      toast.error('Logout failed. Please try again.');
    },
  });
};
