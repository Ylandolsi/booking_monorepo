import { createFileRoute, Outlet } from '@tanstack/react-router';

export const Route = createFileRoute('/store')({
  component: StoreLayout,
});

function StoreLayout() {
  return (
    <div className="min-h-screen">
      <div className="container mx-auto">
        <div className="outlet">
          {/* Outlet is where the child routes are rendered */}
          <Outlet />
        </div>
      </div>
    </div>
  );
}
