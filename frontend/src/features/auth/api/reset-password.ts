import { useMutation } from '@tanstack/react-query';
import { toast } from 'sonner';
import { api } from '@/lib/api-client';
import * as Endpoints from '@/lib/endpoints.ts';
import type { ResetPasswordInput } from '@/features/auth';

const resetPassword = async (data: ResetPasswordInput): Promise<void> =>
  await api.post<void>(Endpoints.ResetPasswordVerify, data);

export const useResetPassword = ({
  onSuccess,
}: { onSuccess?: () => void } = {}) => {
  return useMutation({
    mutationFn: resetPassword,
    onSuccess: () => {
      console.log('password reset successfully');
      onSuccess?.();
      toast.success(
        'Password reset successfully. Please login with your new password.',
      );
    },
    onError: (error) => {
      console.error('Reset password failed:', error);
      toast.error('Failed to reset password. Please try again.');
    },
  });
};
