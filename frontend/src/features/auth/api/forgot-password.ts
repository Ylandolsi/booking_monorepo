import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import * as Endpoints from '@/lib/endpoints.ts';
import type { ForgotPasswordInput } from '@/features/auth';

// Forgot Password
const forgotPassword = async (data: ForgotPasswordInput): Promise<void> => {
  const response = await api.post<void>(Endpoints.ResetPasswordSendToken, data);
  return response;
};

export const useForgotPassword = ({
  onSuccess = () => {},
}: {
  onSuccess?: () => void;
} = {}) => {
  return useMutation({
    mutationFn: forgotPassword,
    meta: {
      invalidatesQuery: [['user']],
      successMessage: 'A reset link is sent to your mail',
      successAction: () => onSuccess(),
      errorMessage: 'Failed to send reset link. Please try again.',
    },
  });
};
