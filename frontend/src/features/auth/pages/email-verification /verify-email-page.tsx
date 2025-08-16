import { AuthLayout, EmailVerificationPage } from '@/features/auth/components';
// import { useNavigate } from '@tanstack/react-router';

export function VerificationEmailPage() {
  // const navigate = useNavigate();
  // const { redirectTo, token, email } = Route.useSearch();

  return (
    <>
      <AuthLayout>
        <EmailVerificationPage
          onSuccess={() => {
            // navigate({
            //   to: redirectTo ? redirectTo : paths.auth.emailVerified.getHref(),
            //   replace: true,
            // });
          }}
          token=""
          email=""
          // token={token}
          // email={email}
        />
      </AuthLayout>
    </>
  );
}
