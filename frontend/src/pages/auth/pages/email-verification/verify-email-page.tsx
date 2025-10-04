import { ROUTE_PATHS } from '@/config';
import { EmailVerificationPage } from '@/pages/auth/components';
import { useAppNavigation } from '@/hooks';
import { getRouteApi } from '@tanstack/react-router';

export function VerificationEmailPage() {
  const navigate = useAppNavigation();
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.EMAIL_VERIFICATION);
  const { redirectTo, token, email } = routeApi.useSearch();
  return (
    <>
      <EmailVerificationPage
        onSuccess={() => {
          if (redirectTo) navigate.goTo({ to: redirectTo, replace: true });
          else navigate.goToEmailVerificationVerified();
        }}
        token={token}
        email={email}
      />
    </>
  );
}
