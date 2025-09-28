import * as z from 'zod';

const createEnv = () => {
  const EnvSchema = z.object({
    API_URL: z.string(),
    APP_URL: z.string().optional().default('http://localhost:3000'),
    VITE_ENVIRONMENT: z.enum(['development', 'staging', 'production']),
  });

  const envVars = {
    API_URL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
    APP_URL: import.meta.env.VITE_APP_URL || 'http://localhost:3000',
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
