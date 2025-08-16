import { createFileRoute } from '@tanstack/react-router';
import { BecomeMentorPage } from '@/features/mentor/become/pages/become-mentor-page';
import { ContentLayout } from '@/components';

export const Route = createFileRoute('/mentor/become')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <BecomeMentorPage />
    </ContentLayout>
  );
}
