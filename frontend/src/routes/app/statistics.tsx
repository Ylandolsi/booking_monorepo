import { StatisticsPage } from '@/pages/app/store/private/statistics';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/statistics')({
  component: RouteComponent,
});

function RouteComponent() {
  return <StatisticsPage />;
}
