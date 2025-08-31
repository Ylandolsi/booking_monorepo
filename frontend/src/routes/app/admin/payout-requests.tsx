import { ROUTE_PATHS } from '@/config';
import { AdminPayoutRequestsPage } from '@/features/admin';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(ROUTE_PATHS.APP.ADMIN.PAYOUT_REQUESTS)({
  component: AdminPayoutRequestsPage,
});