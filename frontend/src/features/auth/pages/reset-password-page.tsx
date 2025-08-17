import { ROUTE_PATHS } from '@/config';
import { ResetPasswordPage } from '@/features/auth/components';
import { useAppNavigation } from '@/hooks';
import { getRouteApi } from '@tanstack/react-router';

export function ResetPassword() {
  const navigate = useAppNavigation();
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.RESET_PASSWORD);
  const { redirectTo, email, token } = routeApi.useSearch();
  return (
    <ResetPasswordPage
      email={email!}
      token={token!}
      onSuccess={() => {
        if (redirectTo) navigate.goTo({ to: redirectTo, replace: true });
        else navigate.goToLogin();
      }}
    />
  );
}
