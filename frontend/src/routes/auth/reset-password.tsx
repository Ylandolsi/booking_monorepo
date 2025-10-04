import { createFileRoute, redirect } from '@tanstack/react-router';
import { ResetPassword } from '@/pages/auth/pages/reset-password-page';
import { ROUTE_PATHS, routeBuilder } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.RESET_PASSWORD)({
  component: ResetPassword,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    email: search.email as string | undefined,
    token: search.token as string | undefined,
  }),
  beforeLoad: ({ search }) => {
    if (!search.email || !search.token) {
      throw redirect({
        to: routeBuilder.auth.login(),
      });
    }
  },
});
