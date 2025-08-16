import { ContentLayout } from '@/components';
import { EnhancedBookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/booking/enhanced/$mentorSlug/')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <EnhancedBookingPage />
    </ContentLayout>
  );
}
