import { useMutation } from '@tanstack/react-query';
import { toast } from 'sonner';
import { api } from '@/lib/api-client';
import * as Endpoints from '@/lib/endpoints.ts';
import type { RegisterInput } from '@/features/auth';

const registerUser = async (data: RegisterInput): Promise<void> =>
  await api.post(Endpoints.Register, data);

export const useRegister = ({
  onSuccess,
}: {
  onSuccess?: () => void;
} = {}) => {
  return useMutation({
    mutationFn: registerUser,
    onSuccess: () => {
      toast.success(
        'Registration successful! Please check your email for verification.',
      );
      onSuccess?.();
    },
    onError: (error) => {
      console.error('Registration failed:', error);
      toast.error('Registration failed. Please try again.');
    },
  });
};
