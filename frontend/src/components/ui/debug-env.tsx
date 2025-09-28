import { env } from '@/config/env';

export const DebugEnv = () => {
  if (env.VITE_ENVIRONMENT !== 'development') return null;

  return (
    <div className="fixed right-4 bottom-4 max-w-sm rounded-lg bg-black/80 p-4 text-xs text-white">
      <h3 className="mb-2 font-bold">Environment Debug</h3>
      <pre>{JSON.stringify(env, null, 2)}</pre>
    </div>
  );
};
