import { createFileRoute } from '@tanstack/react-router';
import { BecomeMentorPage } from '@/features/mentor/become/pages/become-mentor-page';
import { ContentLayout } from '@/components';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.MENTOR.BECOME)({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <BecomeMentorPage />
    </ContentLayout>
  );
}
