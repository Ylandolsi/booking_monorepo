import { PageLoading } from '@/components';
import { createRouter } from '@tanstack/react-router';
import { routeTree } from '@/routeTree.gen';
import { queryClient } from '@/providers/react-query';

export const router = createRouter({
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
