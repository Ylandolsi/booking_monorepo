import { ROUTE_PATHS } from '@/config';
import { RegisterForm } from '@/pages/auth';
import { useAppNavigation } from '@/hooks';
import { getRouteApi } from '@tanstack/react-router';

export function RegisterPage() {
  const navigate = useAppNavigation();
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.REGISTER);
  const { redirectTo } = routeApi.useSearch();
  return (
    <RegisterForm
      OnRegiterSuccess={() => {
        if (redirectTo) navigate.goTo({ to: redirectTo, replace: true });
        else navigate.goToLogin();
      }}
    />
  );
}
