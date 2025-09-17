import { createFileRoute } from '@tanstack/react-router';
import { StoreDashboardPage } from '@/features/store/components/pages/store-dashboard-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.STORE)({
  component: StoreDashboardPage,
});
