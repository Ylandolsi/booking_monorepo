import { useMutation } from '@tanstack/react-query';
import type { User } from '@/types/api';
import { toast } from 'sonner';
import { api } from '@/lib/api/api-client';
import * as Endpoints from '@/api/utils/auth-endpoints';
import { type LoginInput } from '@/features/auth';

export const loginWithEmailAndPassword = async (data: LoginInput): Promise<User> => {
  const response = await api.post<User>(Endpoints.Login, data);
  return response;
};

export const useLogin = ({
  onSuccess,
  onError,
}: {
  onSuccess?: (user: User) => void;
  onError?: (error: Error) => void;
} = {}) => {
  // const queryClient = useQueryClient();

  return useMutation({
    mutationFn: loginWithEmailAndPassword,
    onSuccess: (data) => {
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
