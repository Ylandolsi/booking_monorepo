import { ROUTE_PATHS } from '@/config';
import { ForgotPasswordForm } from '@/pages/auth/components';
import { useAppNavigation } from '@/hooks';
import { getRouteApi } from '@tanstack/react-router';

export function ForgotPasswordPage() {
  const navigate = useAppNavigation();
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.FORGOT_PASSWORD);
  const { redirectTo } = routeApi.useSearch();

  return (
    <ForgotPasswordForm
      onSuccess={() => {
        if (redirectTo) navigate.goTo({ to: redirectTo, replace: true });
        else navigate.goToLogin();
      }}
    />
  );
}
