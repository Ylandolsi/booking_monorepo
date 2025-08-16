// eslint-disable-next-line check-file/filename-naming-convention
import { ErrorComponenet } from '@/components';
import { createRootRoute, NotFoundRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/router-devtools';

export const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools />
    </>
  ),
});

export const notFoundRoute = new NotFoundRoute({
  getParentRoute: () => Route,
  component: ErrorComponenet, // Your 404 component
});
