import { RegisterForm } from '@/features/auth';
import { useNavigate } from '@tanstack/react-router';

export function RegisterPage() {
  // const navigate = useNavigate();
  // const { redirectTo } = Route.useSearch();

  return (
    <RegisterForm
      OnRegiterSuccess={() => {
        return;
      }}
      // OnRegiterSuccess={() => {
      //   const targetPath =
      //     redirectTo || paths.auth.verificationEmail.getHref();
      //   navigate({ to: targetPath });
      // }}
    />
  );
}
