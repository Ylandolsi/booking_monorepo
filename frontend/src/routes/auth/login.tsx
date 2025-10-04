import { createFileRoute } from '@tanstack/react-router';
import { LoginPage } from '@/pages/auth/pages/login-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.LOGIN)({
  component: LoginPage,
  validateSearch: (search: Record<string, unknown>) => {
    // This validation is a safeguard against redirect loops.
    let redirectTo = search.redirectTo as string | undefined;
    if (redirectTo && redirectTo.startsWith('/auth/')) {
      redirectTo = undefined;
    }
    return {
      redirectTo: redirectTo,
    };
  },
});
