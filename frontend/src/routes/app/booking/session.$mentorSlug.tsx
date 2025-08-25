import { createFileRoute } from '@tanstack/react-router';
import { BookingPage } from '@/features/app/session/booking';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.BOOKING.SESSION)({
  component: RouteComponent,
});

function RouteComponent() {
  return <BookingPage />;
}
