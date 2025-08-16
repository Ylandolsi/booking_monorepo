import { ContentLayout } from '@/components';
import { BookingPageDemo } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/booking/test/$mentorSlug/')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <BookingPageDemo />
    </ContentLayout>
  );
}
