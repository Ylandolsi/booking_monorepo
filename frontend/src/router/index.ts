/**
 * Centralized Router Configuration
 * This file contains the root router setup and route tree configuration
 */

import { createRouter } from '@tanstack/react-router';
import { routeTree } from '../routeTree.gen';

// Router configuration
export const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  defaultPreloadStaleTime: 0,
});

// Declare router type for TypeScript
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router;
  }
}

export default router;
