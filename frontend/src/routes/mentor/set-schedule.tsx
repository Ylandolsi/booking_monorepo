import { SetSchedulePage } from '@/features/mentor';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.MENTOR.SET_SCHEDULE)({
  component: RouteComponent,
});

function RouteComponent() {
  return <SetSchedulePage />;
}
