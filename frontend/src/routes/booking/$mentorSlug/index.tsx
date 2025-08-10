import { ContentLayout } from '@/components';
import { BookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/booking/$mentorSlug/')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <BookingPage />
    </ContentLayout>
  );
}
