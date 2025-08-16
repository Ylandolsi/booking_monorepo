import { createFileRoute } from '@tanstack/react-router';
import { RegisterPage } from '@/features/auth/pages/register-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.REGISTER)({
  component: RegisterPage,
  validateSearch: (search: Record<string, unknown>) => ({
    redirectTo: search.redirectTo as string | undefined,
  }),
});
