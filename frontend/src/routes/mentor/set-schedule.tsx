import { SetSchedulePage } from '@/features/mentor';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/mentor/set-schedule')({
  component: RouteComponent,
});

function RouteComponent() {
  return <SetSchedulePage />;
}
