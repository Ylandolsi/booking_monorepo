import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { AuthLayout , RegisterForm } from '@/features/auth'
import { paths } from '@/config/paths';

export const Route = createFileRoute('/auth/register')({
  component: RegisterPage,
  validateSearch: (search: Record<string, unknown>) => ({
    redirectTo: search.redirectTo as string | undefined,
  }),
})

function RegisterPage() {
  const navigate = useNavigate();
  const { redirectTo } = Route.useSearch();

  return (
    <AuthLayout>
      <RegisterForm
        OnRegiterSuccess={() => {
          const targetPath = redirectTo || paths.auth.verificationEmail.getHref();
          navigate({ to: targetPath })
        }}
      />
    </AuthLayout>
  )
}