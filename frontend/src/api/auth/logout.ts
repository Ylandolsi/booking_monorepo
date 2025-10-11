import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toast } from 'sonner';
import { api } from '@/api/utils';
import { authQueryKeys } from '@/api/auth';
import { AuthEndpoints } from '../utils/auth-endpoints';

const logout = async (): Promise<void> => await api.post(AuthEndpoints.Logout);

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
      // toast.error('Logout failed. Please try again.');
    },
  });
};
