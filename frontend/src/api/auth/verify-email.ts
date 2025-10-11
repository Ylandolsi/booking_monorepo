import { api } from '@/api/utils';
import type { VerifyEmailInput } from '@/pages/auth';
import { verifyEmailSchema } from '@/pages/auth';
import { AuthEndpoints } from '../utils/auth-endpoints';

// Email Verification
export const verifyEmail = async (data: VerifyEmailInput): Promise<void> => {
  verifyEmailSchema.parse(data); // TODO : verify this
  await api.post(AuthEndpoints.Email.Verify, data);
};
