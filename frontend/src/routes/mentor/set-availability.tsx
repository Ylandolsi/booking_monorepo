import { SetAvailabilityPage } from '@/features/booking/set-availability';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/mentor/set-availability')({
  component: RouteComponent,
});

function RouteComponent() {
  return <SetAvailabilityPage />;
}
