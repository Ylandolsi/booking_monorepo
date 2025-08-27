import {
  MutationCache,
  QueryClient,
  QueryClientProvider,
  type QueryKey,
} from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { HelmetProvider } from 'react-helmet-async';
import { PageLoading } from '@/components';
import type { DefaultOptions } from '@tanstack/react-query';
import { toast, Toaster } from 'sonner';
import { createRouter, RouterProvider } from '@tanstack/react-router';
import { routeTree } from '@/routeTree.gen';

type AppProviderProps = {
  children?: React.ReactNode;
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

const router = createRouter({
  routeTree,
  context: {
    queryClient, // injected so it can be used in loader
  },
  defaultPendingComponent: () => <PageLoading />,
});

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router;
  }
}

export const AppProvider = ({ children }: AppProviderProps) => {
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
