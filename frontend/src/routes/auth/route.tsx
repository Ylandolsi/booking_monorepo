import { AuthLayout } from '@/pages/auth';
import { Outlet, createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/auth')({
  component: AuthLayoutComponent,
});

function AuthLayoutComponent() {
  return (
    <div>
      <AuthLayout>
        <Outlet />
      </AuthLayout>
    </div>
  );
}
