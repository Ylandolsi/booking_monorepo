import { createFileRoute, redirect } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.INDEX)({
  beforeLoad: () => {
    throw redirect({
      to: ROUTE_PATHS.AUTH.LOGIN,
      search: { redirectTo: undefined },
    });
  },
});
