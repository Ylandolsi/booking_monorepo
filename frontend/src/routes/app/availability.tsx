import { createFileRoute } from '@tanstack/react-router';
import { SetAvailabilityPage } from '@/features/booking';
import { ContentLayout } from '@/components';

export const Route = createFileRoute('/app/availability')({
  component: ppg,
});

export function ppg() {
  return (
    <ContentLayout>
      <SetAvailabilityPage />
    </ContentLayout>
  );
}
