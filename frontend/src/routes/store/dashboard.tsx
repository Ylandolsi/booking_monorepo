import { createFileRoute } from '@tanstack/react-router';
import { StoreDashboardPage } from '@/features/store/components/pages/store-dashboard-page';

export const Route = createFileRoute('/store/dashboard')({
  component: StoreDashboardPage,
});
