import { ROUTE_PATHS } from '@/config';
import { PayoutPage } from '@/pages';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(ROUTE_PATHS.APP.PAYMENT.PAYOUT)({
  component: PayoutPage,
});
