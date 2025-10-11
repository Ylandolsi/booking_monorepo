import { useMutation } from '@tanstack/react-query';
import { toast } from 'sonner';
import { api } from '@/api/utils';
import type { RegisterInput } from '@/pages/auth';
import { AuthEndpoints } from '../utils/auth-endpoints';
import { logger } from '@/lib';

const registerUser = async (data: RegisterInput): Promise<void> => await api.post(AuthEndpoints.Local.Register, data);

export const useRegister = ({
  onSuccess,
}: {
  onSuccess?: () => void;
} = {}) => {
  return useMutation({
    mutationFn: registerUser,
    onSuccess: () => {
      toast.success('Registration successful! Please check your email for verification.');
      onSuccess?.();
    },
    onError: (error) => {
      logger.error('Registration failed:', error);
      // toast.error('Registration failed. Please try again.');
    },
  });
};
