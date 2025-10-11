import * as z from 'zod';
import { logger } from '@/lib';

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
    const errorMessage = `❌ Invalid environment configuration:\n${Object.entries(parsedEnv.error.flatten().fieldErrors)
      .map(([key, errors]) => `  - ${key}: ${errors?.join(', ')}`)
      .join('\n')}`;

    // Only log in development
    if (import.meta.env.DEV) {
      console.error(errorMessage);
      console.error('Received values:', envVars);
    }

    // In production, fail fast with user-friendly error
    if (import.meta.env.PROD) {
      document.body.innerHTML = `
        <div style="display: flex; align-items: center; justify-content: center; height: 100vh; font-family: system-ui; background: #fee; color: #c00; padding: 20px;">
          <div style="max-width: 600px; text-align: center;">
            <h1>Configuration Error</h1>
            <p>The application is not properly configured. Please contact support.</p>
          </div>
        </div>
      `;
      throw new Error('Invalid environment configuration');
    }

    // In development, use fallbacks with warning
    console.warn('⚠️ Using fallback environment variables for development');
    return {
      API_URL: 'http://localhost:5000',
      APP_URL: 'http://localhost:3000',
      VITE_ENVIRONMENT: 'development' as const,
    };
  }

  // Only log success in development
  if (import.meta.env.DEV) {
    console.info('✅ Environment configuration is valid');
  }

  return parsedEnv.data;
};

export const env = createEnv();

// Freeze to prevent mutations
Object.freeze(env);
