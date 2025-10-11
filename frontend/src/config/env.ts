import * as z from 'zod';

const createEnv = () => {
  const EnvSchema = z.object({
    API_URL: z.string().url('API_URL must be a valid URL'),
    APP_URL: z.string().url('APP_URL must be a valid URL'),
    VITE_ENVIRONMENT: z.enum(['development', 'staging', 'production']),
    // Add any other required env vars
  });

  const envVars = {
    API_URL: import.meta.env.VITE_API_URL,
    APP_URL: import.meta.env.VITE_APP_URL,
    VITE_ENVIRONMENT: import.meta.env.VITE_ENVIRONMENT,
  };

  const parsedEnv = EnvSchema.safeParse(envVars);

  if (!parsedEnv.success) {
    // In production, this should fail fast
    const errorMessage = `❌ Invalid environment configuration:\n${Object.entries(parsedEnv.error.flatten().fieldErrors)
      .map(([key, errors]) => `  - ${key}: ${errors?.join(', ')}`)
      .join('\n')}`;

    console.error(errorMessage);

    // Fail fast in production
    if (import.meta.env.PROD) {
      throw new Error('Invalid environment configuration. Check console for details.');
    }

    // In development, show warning but use defaults
    console.warn('Using fallback environment variables for development');
    return {
      API_URL: 'http://localhost:5000',
      APP_URL: 'http://localhost:3000',
      VITE_ENVIRONMENT: 'development' as const,
    };
  }
  console.log('✅ Environment configuration is valid.');

  return parsedEnv.data ?? {};
};

export const env = createEnv();

// Freeze to prevent mutations
Object.freeze(env);
