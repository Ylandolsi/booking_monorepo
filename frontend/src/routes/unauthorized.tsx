import { Unauthorized } from '@/components';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.UNAUTHORIZED)({
  component: Unauthorized,
});
