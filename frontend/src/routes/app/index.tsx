import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';
import { HomePage } from '@/components';

export const Route = createFileRoute(ROUTE_PATHS.APP.INDEX)({
  component: HomePage,
});
