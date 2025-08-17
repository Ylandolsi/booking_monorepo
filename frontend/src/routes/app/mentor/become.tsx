import { createFileRoute } from '@tanstack/react-router';
import { BecomeMentorPage } from '@/features/app/mentor/become/pages/become-mentor-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.MENTOR.BECOME)({
  component: RouteComponent,
});

function RouteComponent() {
  return <BecomeMentorPage />;
}
