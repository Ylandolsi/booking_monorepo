import { api } from '@/api/utils';
import * as Endpoints from '@/api/utils/auth-endpoints';
import type { VerifyEmailInput } from '@/features/auth';
import { verifyEmailSchema } from '@/features/auth';

// Email Verification
export const verifyEmail = async (data: VerifyEmailInput): Promise<void> => {
  verifyEmailSchema.parse(data); // TODO : verify this
  await api.post(Endpoints.VerifyEmail, data);
};
