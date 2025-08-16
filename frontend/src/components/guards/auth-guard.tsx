import { Navigate, useLocation } from '@tanstack/react-router';
import { useUser } from '@/features/auth';
import { Spinner } from '@/components/ui';
import { routes } from '@/config/routes';
import type { ReactNode } from 'react';

interface AuthGuardProps {
  children: ReactNode;
  requireAuth?: boolean;
  requireAdmin?: boolean;
  authPage?: boolean;
  redirectTo?: string;
}

export const AuthGuard = ({
  children,
  requireAuth = false,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  requireAdmin = false,
  authPage = false,
  redirectTo = routes.to.auth.login(),
}: AuthGuardProps) => {
  const { data: user, isLoading, error } = useUser();
  const location = useLocation();

  if (isLoading && (requireAuth || authPage)) {
    return (
      <div className="flex h-screen items-center justify-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (authPage) {
    if (user) {
      return <Navigate to={'/app' as any} replace />;
    }
    return <>{children}</>;
  }

  if (!requireAuth) {
    return <>{children}</>;
  }

  if (!user || error) {
    if (isAuthPath(location.pathname)) {
      return <Navigate to={redirectTo} replace />;
    }

    const cleanPath = location.pathname;
    const loginUrl = routes.to.auth.login({ redirectTo: cleanPath });
    return <Navigate to={loginUrl as any} replace />;
  }

  return <>{children}</>;
};

const isAuthPath = (path: string) => {
  return path.includes('/auth/');
};
