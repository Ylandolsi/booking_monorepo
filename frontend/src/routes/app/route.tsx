import { ContentLayout } from '@/components';
import { createFileRoute, Outlet } from '@tanstack/react-router';

export const Route = createFileRoute('/app')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <Outlet />
    </ContentLayout>
  );
}
