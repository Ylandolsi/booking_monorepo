import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';
import { VerificationEmailDonePage } from '@/features/auth/pages/email-verification ';

export const Route = createFileRoute(
  ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED,
)({
  component: VerificationEmailDonePage,
});
