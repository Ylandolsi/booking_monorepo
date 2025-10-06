import { ROUTE_PATHS } from '@/config';
import { PayoutPage } from '@/pages/store';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(ROUTE_PATHS.APP.PAYMENT.PAYOUT)({
  component: PayoutPage,
});
