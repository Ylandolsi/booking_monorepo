import { createFileRoute, useNavigate, redirect } from '@tanstack/react-router';
import { AuthLayout, ResetPasswordPage } from '@/features/auth';
import { paths } from '@/config/paths';

export const Route = createFileRoute('/auth/reset-password')({
  component: ResetPassword,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    email: search.email as string | undefined,
    token: search.token as string | undefined,
  }),
  beforeLoad: ({ search }) => {
    if (!search.email || !search.token) {
      throw redirect({
        to: paths.auth.login.getHref(),
      });
    }
  },
});

function ResetPassword() {
  const navigate = useNavigate();
  const { redirectTo, email, token } = Route.useSearch();

  return (
    <AuthLayout>
      <ResetPasswordPage
        email={email!}
        token={token!}
        onSuccess={() => {
          const targetPath = redirectTo || paths.auth.login.getHref();
          navigate({ to: targetPath });
        }}
      />
    </AuthLayout>
  );
}
