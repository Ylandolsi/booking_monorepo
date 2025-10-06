import { createFileRoute } from '@tanstack/react-router'
import { StatisticsPage } from '@/pages/store/private/statistics';

export const Route = createFileRoute('/app/statistics')({
  component: RouteComponent,
});

function RouteComponent() {
  return <StatisticsPage />;
}
