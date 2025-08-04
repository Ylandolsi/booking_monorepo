import * as z from 'zod';

const createEnv = () => {
  const EnvSchema = z.object({
    API_URL: z.string(),
    ENABLE_API_MOCKING: z
      .string()
      .refine((s) => s === 'true' || s === 'false')
      .transform((s) => s === 'true')
      .optional(),
    REFRESH_URL: z
      .string()
      .optional()
      .default('http://localhost:3000/users/refresh-token'),
    APP_URL: z.string().optional().default('http://localhost:3000'),
    APP_MOCK_API_PORT: z.string().optional().default('8080'),
    JWT_ISSUER: z.string().optional(),
    JWT_AUDIENCE: z.string().optional(),
    AUTH_SERVICE_URL: z.string().optional().default('http://localhost:3000'),
  });

  const envVars = {
    API_URL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
    REFRESH_URL:
      import.meta.env.VITE_REFRESH_URL ||
      'http://localhost:5000/users/refresh-token',
    ENABLE_API_MOCKING: import.meta.env.VITE_ENABLE_API_MOCKING,
    APP_URL: import.meta.env.VITE_APP_URL || 'http://localhost:3000',
    APP_MOCK_API_PORT: import.meta.env.VITE_MOCK_API_PORT,
    JWT_ISSUER: import.meta.env.VITE_JWT_ISSUER,
    JWT_AUDIENCE: import.meta.env.VITE_JWT_AUDIENCE,
    AUTH_SERVICE_URL:
      import.meta.env.VITE_AUTH_SERVICE_URL || 'http://localhost:3000',
  };

  const parsedEnv = EnvSchema.safeParse(envVars);

  if (!parsedEnv.success) {
    throw new Error(
      `Invalid environment configuration:\n${Object.entries(
        parsedEnv.error.flatten().fieldErrors,
      )
        .map(([key, errors]) => `- ${key}: ${errors.join(', ')}`)
        .join('\n')}`,
    );
  }

  return parsedEnv.data ?? {};
};

export const env = createEnv();
console.log('Environment variables loaded successfully:', env);
