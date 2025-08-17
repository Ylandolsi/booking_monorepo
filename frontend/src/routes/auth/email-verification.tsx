import { createFileRoute } from '@tanstack/react-router';
import { VerificationEmailPage } from '@/features/auth/pages/email-verification/verify-email-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.AUTH.EMAIL_VERIFICATION)({
  component: VerificationEmailPage,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    token: search.token as string | undefined,
    email: search.email as string | undefined,
  }),
});
