import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/booking/session/$mentorSlug/')({
  component: RouteComponent,
});

function RouteComponent() {
  return <BookingPage />;
}
