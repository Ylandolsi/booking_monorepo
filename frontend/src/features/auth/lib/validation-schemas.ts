import { z } from 'zod';

// Login Schema
export const loginInputSchema = z.object({
  email: z.string().min(1, 'Required').email('Invalid email'),
  password: z.string().min(8, 'Password must be at least 8 characters'),
});

// Register Schema
export const registerInputSchema = z
  .object({
    email: z.string().email('Invalid email').min(1, 'Required'),
    firstName: z.string().min(2, 'Required'),
    lastName: z.string().min(2, 'Required'),
    password: z
      .string()
      .min(8, 'Password must be at least 8 characters')
      .regex(
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$/,
        'Password must contain at least one uppercase letter, one lowercase letter, and one number',
      ),
    confirmPassword: z.string().min(8, 'Required'),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ['confirmPassword'],
  });

// Forgot Password Schema
export const forgotPasswordInputSchema = z.object({
  email: z.string().min(1, 'Required').email('Invalid email'),
});

// Reset Password Schema
export const resetPasswordSchema = z
  .object({
    email: z.string().email('Invalid email').min(1, 'Required'),
    password: z
      .string()
      .min(8, 'Password must be at least 8 characters')
      .regex(
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$/,
        'Password must contain at least one uppercase letter, one lowercase letter, and one number',
      ),
    confirmPassword: z.string().min(8, 'Required'),
    token: z.string().min(1, 'token required'),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ['confirmPassword'],
  });

// Email Verification Schema
export const verifyEmailSchema = z.object({
  email: z.string().email('Invalid email').min(1, 'Required'),
  token: z.string().min(2, 'Required'),
});
