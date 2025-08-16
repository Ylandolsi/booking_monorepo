import {
  MutationCache,
  QueryClient,
  QueryClientProvider,
  type QueryKey,
} from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import { HelmetProvider } from 'react-helmet-async';
import { MainErrorFallback } from '@/components/errors/main';
import { PageLoading } from '@/components';
import type { DefaultOptions } from '@tanstack/react-query';
import { toast, Toaster } from 'sonner';

type AppProviderProps = {
  children: React.ReactNode;
};
// meta? : Register["mutationMeta"];
declare module '@tanstack/react-query' {
  interface Register {
    mutationMeta: {
      invalidatesQuery?: QueryKey | QueryKey[];
      successMessage?: string;
      errorMessage?: string;

      successAction?: (data: any) => void;
      errorAction?: (data: any) => void;
    };
  }
}

const queryConfig = {
  queries: {
    refetchOnWindowFocus: false, // Don't refetch data when window regains focus
    retry: 0, //  Dont Retry failed queries , TODO : maybe add retry ?
    staleTime: 1000 * 60 * 5, // Data is considered fresh for 5 minutes
    gcTime: 1000 * 60 * 10, // Unused data is garbage collected after 10 minutes
  },
  mutations: {
    retry: 0, // Don't retry failed mutations
  },
} satisfies DefaultOptions;

const queryClient = new QueryClient({
  defaultOptions: queryConfig,
  mutationCache: new MutationCache({
    onSuccess: (data, _variables, _context, mutation) => {
      if (mutation.meta?.successMessage) {
        toast.success(mutation.meta.successMessage);
      }
      if (mutation.meta?.successAction) {
        mutation.meta.successAction(data);
      }
    },
    onError: (error, _variables, _context, mutation) => {
      if (mutation.meta?.errorMessage) {
        toast.error(mutation.meta.errorMessage);
      }
      if (mutation.meta?.errorAction) {
        mutation.meta.errorAction(error);
      }
    },
    onSettled: (_data, _error, _variables, _context, mutation) => {
      if (mutation.meta?.invalidatesQuery) {
        const keys = Array.isArray(mutation.meta.invalidatesQuery)
          ? mutation.meta.invalidatesQuery
          : [mutation.meta.invalidatesQuery];

        keys.forEach((key) => {
          queryClient.invalidateQueries({ queryKey: key });
        });
      }
    },
  }),
});
export const AppProvider = ({ children }: AppProviderProps) => {
  return (
    <React.Suspense fallback={<PageLoading />}>
      <ErrorBoundary FallbackComponent={MainErrorFallback}>
        <HelmetProvider>
          <QueryClientProvider client={queryClient}>
            {import.meta.env.DEV && <ReactQueryDevtools />}
            {/* <TanStackRouterDevtools /> */}
            <Toaster />
            {children}
          </QueryClientProvider>
        </HelmetProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
