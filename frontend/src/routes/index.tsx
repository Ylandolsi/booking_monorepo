import { createFileRoute } from '@tanstack/react-router';
import { HomePage } from '@/components/pages/home';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.ROOT)({
  component: HomePage,
});
