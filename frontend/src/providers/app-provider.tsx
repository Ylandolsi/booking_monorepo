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
import { Spinner } from '@/components/ui/spinner';
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
      successAction?: () => void;
      errorMessage?: string;
    };
  }
}

const queryConfig = {
  queries: {
    refetchOnWindowFocus: false, // Don't refetch data when window regains focus
    retry: 1, // Retry failed queries once
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
    onSuccess: (_data, _variables, _context, mutation) => {
      if (mutation.meta?.successMessage) {
        toast.success(mutation.meta.successMessage);
      }
      if (mutation.meta?.successAction) {
        mutation.meta.successAction();
      }
    },
    onError: (_error, _variables, _context, mutation) => {
      if (mutation.meta?.errorMessage) {
        toast.error(mutation.meta.errorMessage);
      }
    },
    onSettled: (_data, _error, _variables, _context, mutation) => {
      if (mutation.meta?.invalidatesQuery) {
        const keys = Array.isArray(mutation.meta.invalidatesQuery)
          ? mutation.meta.invalidatesQuery
          : [mutation.meta.invalidatesQuery];

        keys.forEach((key: QueryKey) => {
          queryClient.invalidateQueries({ queryKey: key });
        });
      }
    },
  }),
});
export const AppProvider = ({ children }: AppProviderProps) => {
  // const [queryClient] = React.useState(
  //   () =>
  //     new QueryClient({
  //       defaultOptions: queryConfig,
  //     }),
  // );

  return (
    <React.Suspense
      fallback={
        <div className="flex h-screen w-screen items-center justify-center">
          <Spinner size="xl" />
        </div>
      }
    >
      <ErrorBoundary FallbackComponent={MainErrorFallback}>
        <HelmetProvider>
          <QueryClientProvider client={queryClient}>
            {import.meta.env.DEV && <ReactQueryDevtools />}
            <Toaster />
            {children}
          </QueryClientProvider>
        </HelmetProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
