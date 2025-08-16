import { ContentLayout } from '@/components';
import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

function RouteComponent() {
  return (
    <ContentLayout>
      <BookingPage />
    </ContentLayout>
  );
}

export const Route = createFileRoute('/booking/real/$mentorSlug/')({
  component: RouteComponent,
});
