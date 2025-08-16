import { createFileRoute } from '@tanstack/react-router';
import { MentorRequired } from '@/components/errors';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.TEST.MENTOR_REQUIRED)({
  component: RouteComponent,
});

function RouteComponent() {
  return <MentorRequiredDemo />;
}

export const MentorRequiredDemo = () => {
  return <MentorRequired />;
};
