import { createFileRoute } from '@tanstack/react-router';
import { MentorRequired } from '@/components/errors';

export const Route = createFileRoute('/test/mentor-required')({
  component: RouteComponent,
});

function RouteComponent() {
  return <MentorRequiredDemo />;
}

export const MentorRequiredDemo = () => {
  return <MentorRequired />;
};
