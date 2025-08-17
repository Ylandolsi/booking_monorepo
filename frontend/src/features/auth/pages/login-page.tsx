import { LoginForm } from '@/features/auth/components';
import { useNavigate, useSearch } from '@tanstack/react-router';

export function LoginPage() {
  // const navigate = useNavigate();
  // const { redirectTo } = useSearch();

  return (
    <LoginForm
      onSuccess={() => {
        return;
      }}
      // onSuccess={() => {
      //   const targetPath = redirectTo || paths.app.root.getHref();
      //   navigate({ to: targetPath, replace: true });
      // }}
    />
  );
}
