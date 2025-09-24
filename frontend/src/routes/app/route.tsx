import { ContentLayout } from '@/components';
import { ROUTE_PATHS } from '@/config';
import { createFileRoute, Outlet } from '@tanstack/react-router';
import { useLocation } from '@tanstack/react-router';

export const Route = createFileRoute('/app')({
  component: RouteComponent,
});
function RouteComponent() {
  const location = useLocation();
  const excludedPaths = [ROUTE_PATHS.APP.STORE.SETUP_STORE]; // prevent layout on setup store page
  const shouldUseLayout = !excludedPaths.some((path) => location.pathname.startsWith(path));
  return shouldUseLayout ? (
    <ContentLayout>
      <Outlet />
    </ContentLayout>
  ) : (
    <Outlet />
  );
}
