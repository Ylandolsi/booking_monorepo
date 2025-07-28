// eslint-disable-next-line check-file/filename-naming-convention
import { NotFound } from '@/components';
import { createRootRoute, NotFoundRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/router-devtools';

const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools />
    </>
  ),
});

const notFoundRoute = new NotFoundRoute({
  getParentRoute: () => Route,
  component: NotFound, // Your 404 component
});

export { Route, notFoundRoute };
