import { ContentLayout } from '@/components';
import { EnhancedBookingPage } from '@/features/booking';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/booking/demo/$mentorSlug/')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <div className="mb-4 p-4 bg-yellow-50 border border-yellow-200 rounded-lg">
        <p className="text-sm text-yellow-800">
          <strong>Demo Mode:</strong> This is the enhanced booking page with
          improved UI, better error handling, and cleaner code structure.
        </p>
      </div>
      <EnhancedBookingPage />
    </ContentLayout>
  );
}
