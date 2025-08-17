import { ForgotPasswordForm } from '@/features/auth/components';
// import { useNavigate } from '@tanstack/react-router';

export function ForgotPasswordPage() {
  // const navigate = useNavigate();
  // const { redirectTo } = Route.useSearch();

  return (
    <ForgotPasswordForm
      onSuccess={() => {
        return;
      }}
      // onSuccess={() => {
      //   const targetPath = redirectTo || paths.auth.login.getHref();
      //   navigate({ to: targetPath });
      // }}
    />
  );
}
