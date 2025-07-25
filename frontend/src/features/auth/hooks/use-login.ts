import { useMutation, useQueryClient } from '@tanstack/react-query';
import { loginWithEmailAndPassword } from '@/features/auth';
import type { User } from '@/types/api';
import { toast } from 'sonner';

export const useLogin = ({
  onSuccess,
  onError,
}: {
  onSuccess?: (user: User) => void;
  onError?: (error: Error) => void;
} = {}) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: loginWithEmailAndPassword,
    onSuccess: (data) => {
      queryClient.setQueryData(['user'], data);
      toast.success('Login successful!');
      onSuccess?.(data);
    },
    onError: (error) => {
      console.error('Login failed:', error);
      toast.error('Login failed. Please check your credentials.');
      onError?.(error);
    },
  });
};
