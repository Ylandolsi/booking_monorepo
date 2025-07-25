import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { AuthLayout ,LoginForm } from '@/features/auth';
import { paths } from '@/config/paths';

export const Route = createFileRoute('/auth/login')({
  component: LoginPage,
  validateSearch: (search: Record<string, unknown>) => {
    // This validation is a safeguard against redirect loops.
    let redirectTo = search.redirectTo as string | undefined;
    if (redirectTo && redirectTo.startsWith('/auth/')) {
      redirectTo = undefined;
    }
    return {
      redirectTo: redirectTo,
    };
  },
});

function LoginPage() {
  const navigate = useNavigate();
  const { redirectTo } = Route.useSearch();

  return (
    <AuthLayout>
        <LoginForm
          onSuccess={() => {
            const targetPath = redirectTo || paths.app.root.getHref();
            navigate({ to: targetPath, replace: true });
          }}
        />
    </AuthLayout>
  );
}