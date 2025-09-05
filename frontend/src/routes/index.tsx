import { createFileRoute } from '@tanstack/react-router';
import { LandingPage } from '@/components/pages/landing-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.ROOT)({
  component: LandingPage,
});
