import { ContentLayout } from '@/components';
import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.TEST.BOOKING_DEMO)({
  component: BookingDemo,
});

function BookingDemo() {
  return (
    <ContentLayout>
      <BookingPage />
    </ContentLayout>
  );
}
