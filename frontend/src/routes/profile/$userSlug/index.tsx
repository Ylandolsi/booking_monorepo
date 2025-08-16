import { Profile } from '@/features/profile';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.PROFILE.USER)({
  component: RouteComponent,
});

function RouteComponent() {
  return <Profile />;
}
