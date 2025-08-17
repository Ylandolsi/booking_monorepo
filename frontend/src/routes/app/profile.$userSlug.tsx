import { Profile } from '@/features/app/profile';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.PROFILE.USER)({
  component: RouteComponent,
});

function RouteComponent() {
  return <Profile />;
}
