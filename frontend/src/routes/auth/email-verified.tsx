import { ROUTE_PATHS } from '@/config/routes';
import { VerificationEmailDonePage } from '@/features/auth/pages';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(
  ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED,
)({
  component: VerificationEmailDonePage,
});
