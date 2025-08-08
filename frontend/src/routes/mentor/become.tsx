import { createFileRoute } from '@tanstack/react-router';
import { BecomeMentorPage } from '@/features/mentor/pages/become-mentor-page.tsx';
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
