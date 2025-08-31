import { AdminPayoutRequestDetailsPage } from '@/features/admin';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/admin/payout-requests/$requestId')({
  component: AdminPayoutRequestDetailsPage,
});