import * as z from 'zod';

const createEnv = () => {
  const EnvSchema = z.object({
    API_URL: z.string(),
    APP_URL: z.string().optional().default('http://localhost:3000'),
    APP_MOCK_API_PORT: z.string().optional().default('8080'),
    VITE_ENVIRONMENT: z.enum(['development', 'staging', 'production']),
    ENABLE_API_MOCKING: z
      .string()
      .refine((s) => s === 'true' || s === 'false')
      .transform((s) => s === 'true')
      .optional(),
  });

  const envVars = {
    API_URL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
    ENABLE_API_MOCKING: import.meta.env.VITE_ENABLE_API_MOCKING,
    APP_URL: import.meta.env.VITE_APP_URL || 'http://localhost:3000',
    APP_MOCK_API_PORT: import.meta.env.VITE_MOCK_API_PORT,
    VITE_ENVIRONMENT: import.meta.env.VITE_ENVIRONMENT || 'development',
  };

  const parsedEnv = EnvSchema.safeParse(envVars);

  if (!parsedEnv.success) {
    throw new Error(
      `Invalid environment configuration:\n${Object.entries(parsedEnv.error.flatten().fieldErrors)
        .map(([key, errors]) => `- ${key}: ${errors.join(', ')}`)
        .join('\n')}`,
    );
  }

  return parsedEnv.data ?? {};
};

export const env = createEnv();
console.log('Environment variables loaded successfully:', env);
