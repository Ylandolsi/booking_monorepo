import { AuthLayout, ResetPasswordPage } from '@/features/auth/components';
import { useNavigate } from '@tanstack/react-router';

export function ResetPassword() {
  // const navigate = useNavigate();
  // const { redirectTo, email, token } = Route.useSearch();

  return (
    <AuthLayout>
      <ResetPasswordPage
        // email={email!}
        // token={token!}
        // onSuccess={() => {
        //   const targetPath = redirectTo || paths.auth.login.getHref();
        //   navigate({ to: targetPath });
        // }}
        onSuccess={() => {}}
        token=""
        email=""
      />
    </AuthLayout>
  );
}
