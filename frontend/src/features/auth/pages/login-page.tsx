import { ROUTE_PATHS } from '@/config';
import { LoginForm } from '@/features/auth/components';
import { useAppNavigation } from '@/hooks';
import { getRouteApi } from '@tanstack/react-router';

export function LoginPage() {
  // const { redirectTo } = useSearch();
  const navigate = useAppNavigation();
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.LOGIN);
  const { redirectTo } = routeApi.useSearch();
  return (
    <LoginForm
      onSuccess={() => {
        const targetPath = redirectTo;
        navigate.goTo({ to: targetPath, replace: true });
      }}
    />
  );
}
