import { createFileRoute, Outlet } from '@tanstack/react-router'
import { AuthGuard } from '@/features/auth/components/auth-guard'
// import { AppLayout } from '@/components/layouts/app-layout'

export const Route = createFileRoute('/app')({
  component: AppLayoutComponent,
})

function AppLayoutComponent() {
  return (
    <AuthGuard requireAuth>
      {/* <AppLayout> */}
        <Outlet />
      {/* </AppLayout> */}
    </AuthGuard>
  )
}