import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { AuthLayout , ForgotPasswordForm } from '@/features/auth'
import { paths } from '@/config/paths'

export const Route = createFileRoute('/auth/forgot-password')({
  component: ForgotPasswordPage,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
  }),
})

function ForgotPasswordPage() {
  const navigate = useNavigate()
  const { redirectTo } = Route.useSearch()

  return (
    <AuthLayout>
      <ForgotPasswordForm
        onSuccess={() => {
          const targetPath = redirectTo || paths.auth.login.getHref()
          navigate({ to: targetPath })
        }}
      />
    </AuthLayout>
  )
}