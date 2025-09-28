import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { HelmetProvider } from 'react-helmet-async';
import { PageLoading, useThemeStore } from '@/components';
import { Toaster } from 'sonner';
import { RouterProvider } from '@tanstack/react-router';
import { useEffect } from 'react';
import { queryClient } from '@/providers/react-query';
import { router } from '@/providers/react-router';

type AppProviderProps = {
  children?: React.ReactNode;
};

export const AppProvider = ({ children }: AppProviderProps) => {
  // Set initial theme on app load
  useEffect(() => {
    const { theme } = useThemeStore.getState();
    document.documentElement.classList.toggle('dark', theme === 'dark');
  }, []);

  return (
    // TODO : add React.ErrorBoundary
    <React.Suspense fallback={<PageLoading />}>
      <HelmetProvider>
        <QueryClientProvider client={queryClient}>
          <RouterProvider router={router} />

          {/* < QueryErrorResetBoundary > // TODO : GOOGLE THIS  */}
          {import.meta.env.DEV && <ReactQueryDevtools />}
          <Toaster />
          {children}
        </QueryClientProvider>
      </HelmetProvider>
    </React.Suspense>
  );
};
