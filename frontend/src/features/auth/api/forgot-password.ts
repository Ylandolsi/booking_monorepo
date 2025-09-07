import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api/api-client';
import * as Endpoints from '@/lib/api/user-endpoints';
import type { ForgotPasswordInput } from '@/features/auth';
import { authQueryKeys } from '@/features/auth';

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
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'A reset link is sent to your mail',
      successAction: () => onSuccess(),
      errorMessage: 'Failed to send reset link. Please try again.',
    },
  });
};
