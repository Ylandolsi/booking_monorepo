import { z } from 'zod';
import { loginInputSchema, registerInputSchema, forgotPasswordInputSchema, resetPasswordSchema, verifyEmailSchema } from '../lib/validation-schemas';
import type { User } from '@/api/auth';

export type LoginInput = z.infer<typeof loginInputSchema>;
export type RegisterInput = z.infer<typeof registerInputSchema>;
export type ForgotPasswordInput = z.infer<typeof forgotPasswordInputSchema>;
export type ResetPasswordInput = z.infer<typeof resetPasswordSchema>;
export type VerifyEmailInput = z.infer<typeof verifyEmailSchema>;

export interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
}

export interface AuthContextType extends AuthState {
  login: (data: LoginInput) => Promise<void>;
  register: (data: RegisterInput) => Promise<void>;
  logout: () => Promise<void>;
  forgotPassword: (data: ForgotPasswordInput) => Promise<void>;
  resetPassword: (data: ResetPasswordInput) => Promise<void>;
}
