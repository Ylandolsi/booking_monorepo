import { Navigate, useLocation } from '@tanstack/react-router'
import { useUser } from '@/features/auth'
import { Spinner } from '@/components/ui/spinner'
import type { ReactNode } from 'react'

interface AuthGuardProps {
  children: ReactNode
  requireAuth?: boolean
  requireAdmin?: boolean
  authPage?: boolean
  redirectTo?: string
}

export const AuthGuard = ({
  children,
  requireAuth = false,
  requireAdmin = false,
  authPage = false,
  redirectTo = '/auth/login',
}: AuthGuardProps) => {
  const { data: user, isLoading, error } = useUser()
  const location = useLocation()
  
  if (isLoading && (requireAuth || authPage)) {
    return (
      <div className="flex h-screen items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }
  
  if (authPage) {
    if (user) {
      return <Navigate to="/app" replace />
    }
    return <>{children}</>
  }
  
  if (!requireAuth) {
    return <>{children}</>
  }
  
  if (!user || error) {
    if (isAuthPath(location.pathname)) {
      return <Navigate to={redirectTo} replace />
    }

    const cleanPath = location.pathname
    const loginUrl = `${redirectTo}?redirectTo=${encodeURIComponent(cleanPath)}`
    return <Navigate to={loginUrl} replace />
  }
  
  return <>{children}</>
}

const isAuthPath = (path: string) => {
  return path.includes('/auth/')
}