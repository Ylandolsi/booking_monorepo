import { useMutation } from '@tanstack/react-query';
import type { User } from '@/api/auth';
import { toast } from 'sonner';
import { api } from '@/api/utils';
import { type LoginInput } from '@/pages/auth';
import { AuthEndpoints } from '../utils/auth-endpoints';

export const loginWithEmailAndPassword = async (data: LoginInput): Promise<User> => {
  const response = await api.post<User>(AuthEndpoints.Local.Login, data);
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
