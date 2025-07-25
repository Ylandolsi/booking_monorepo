import { useMutation } from '@tanstack/react-query';
import { forgotPassword } from '@/features/auth';

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
