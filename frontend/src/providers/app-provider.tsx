import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { HelmetProvider } from 'react-helmet-async';
import { PageLoading } from '@/components';
import { Toaster } from 'sonner';
import { RouterProvider } from '@tanstack/react-router';
import { useEffect } from 'react';
import { queryClient } from '@/providers/react-query';
import { router } from '@/providers/react-router';
import { useThemeStore } from '@/stores/theme-store';
import { ErrorBoundary } from '@/providers/error-boundary';
import { useTheme } from '@/hooks';

type AppProviderProps = {
  children?: React.ReactNode;
};

export const AppProvider = ({ children }: AppProviderProps) => {
  // Set initial theme on app load
  const { theme } = useTheme();

  return (
    <HelmetProvider>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />

        {/* < QueryErrorResetBoundary > // TODO : GOOGLE THIS  */}
        {import.meta.env.DEV && <ReactQueryDevtools />}
        <Toaster />
        <React.Suspense fallback={<PageLoading />}>
          <ErrorBoundary showDetails={import.meta.env.MODE === 'development'}>{children}</ErrorBoundary>
        </React.Suspense>
      </QueryClientProvider>
    </HelmetProvider>
  );
};
