import { createFileRoute } from '@tanstack/react-router';
import { ForgotPasswordPage } from '@/pages/auth/pages/forgot-password-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.FORGOT_PASSWORD)({
  component: ForgotPasswordPage,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
  }),
});
