import { createFileRoute, Outlet } from '@tanstack/react-router';
import { AuthGuard, StoreGuard } from '@/components/guards';
import { ROUTE_PATHS } from '@/config/routes';
// import { AppLayout } from '@/components/layouts/app-layout'

export const Route = createFileRoute(ROUTE_PATHS.APP.INDEX)({
  component: AppLayoutComponent,
});

function AppLayoutComponent() {
  return (
    <AuthGuard requireAuth>
      <StoreGuard>
        {/* <AppLayout> */}
        <Outlet />
        {/* </AppLayout> */}
      </StoreGuard>
    </AuthGuard>
  );
}
