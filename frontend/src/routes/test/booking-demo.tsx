import { ContentLayout } from '@/components';
import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/test/booking-demo')({
  component: BookingDemo,
});

function BookingDemo() {
  return (
    <ContentLayout>
      <BookingPage />
    </ContentLayout>
  );
}
