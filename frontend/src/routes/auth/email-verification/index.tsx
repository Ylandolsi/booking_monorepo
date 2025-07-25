import { paths } from '@/config/paths';
import { EmailVerificationPage , AuthLayout } from '@/features/auth';
import { createFileRoute, useNavigate } from '@tanstack/react-router';

export const Route = createFileRoute('/auth/email-verification/')({
  component: VerificationEmailPage,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    token: search.token as string | undefined,
    email: search.email as string | undefined,
  }),
});

function VerificationEmailPage() {
  const navigate = useNavigate();
  const { redirectTo, token, email } = Route.useSearch();

  return (
    <>
      <AuthLayout>
        <EmailVerificationPage
          onSuccess={() => {
            navigate({
              to: redirectTo ? redirectTo : paths.auth.emailVerified.getHref(),
              replace: true,
            });
          }}
          token={token}
          email={email}
        />
      </AuthLayout>
    </>
  );
}
