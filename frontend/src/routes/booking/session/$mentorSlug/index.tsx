import { createFileRoute } from '@tanstack/react-router';
import { BookingPage } from '@/features/booking';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.BOOKING.SESSION)({
  component: RouteComponent,
});

function RouteComponent() {
  return <BookingPage />;
}
