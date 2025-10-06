import { AnalyticsPage } from '@/api/stores';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/statistics')({
  component: RouteComponent,
});

function RouteComponent() {
  return <AnalyticsPage />;
}
