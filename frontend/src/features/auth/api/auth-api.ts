import { api } from '@/lib/api-client';
import * as Endpoints from '@/lib/endpoints.ts';
import type { User } from '@/types/api';
import type {
  LoginInput,
  RegisterInput,
  ForgotPasswordInput,
  ResetPasswordInput,
  VerifyEmailInput,
} from '@/features/auth';
import { verifyEmailSchema } from '@/features/auth';

// Login
export const loginWithEmailAndPassword = async (
  data: LoginInput,
): Promise<User> => {
  const response = await api.post<User>(Endpoints.Login, data);
  return response;
};

// Register
export const registerUser = async (data: RegisterInput): Promise<void> =>
  await api.post(Endpoints.Register, data);

// Logout
export const logout = async (): Promise<void> =>
  await api.post(Endpoints.Logout);

// Forgot Password
export const forgotPassword = async (
  data: ForgotPasswordInput,
): Promise<void> => {
  const response = await api.post<void>(Endpoints.ResetPasswordSendToken, data);
  return response;
};

// Reset Password
export const resetPassword = async (data: ResetPasswordInput): Promise<void> =>
  await api.post<void>(Endpoints.ResetPasswordVerify, data);

// Email Verification
export const verifyEmail = async (data: VerifyEmailInput): Promise<void> => {
  verifyEmailSchema.parse(data); // TODO : verify this
  await api.post(Endpoints.VerifyEmail, data);
};

// Get Current User
export const getCurrentUser = async (): Promise<User> => {
  const response = await api.get<User>(Endpoints.GetCurrentUser);
  return response;
};
