import { Button } from '@/components/ui';
import { Shield, LogIn, Lock } from 'lucide-react';
import { useNavigate } from '@tanstack/react-router';

interface UnauthorizedProps {
  title?: string;
  message?: string;
  showLoginButton?: boolean;
  showHomeButton?: boolean;
}

export const Unauthorized = ({
  title = 'Access Denied',
  message = "You don't have permission to access this page. Please log in or contact an administrator.",
  showLoginButton = true,
  showHomeButton = true,
}: UnauthorizedProps) => {
  const navigate = useNavigate();

  const handleLogin = () => {
    navigate({ to: '/auth/login', search: {} });
  };

  const handleGoHome = () => {
    navigate({ to: '/app', search: {} });
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <div className="max-w-md w-full text-center space-y-6">
        {/* Unauthorized Icon */}
        <div className="mx-auto w-16 h-16 bg-red-100 dark:bg-red-900/20 rounded-full flex items-center justify-center">
          <Shield className="w-8 h-8 text-red-600 dark:text-red-400" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-2xl font-bold text-foreground">{title}</h1>
          <p className="text-muted-foreground">{message}</p>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          {showLoginButton && (
            <Button onClick={handleLogin} className="flex items-center gap-2">
              <LogIn className="w-4 h-4" />
              Log In
            </Button>
          )}

          {showHomeButton && (
            <Button
              variant="outline"
              onClick={handleGoHome}
              className="flex items-center gap-2"
            >
              <Lock className="w-4 h-4" />
              Go Home
            </Button>
          )}
        </div>

        {/* Additional Info */}
        <div className="text-xs text-muted-foreground space-y-1">
          <p>If you believe this is an error:</p>
          <ul className="text-left space-y-1">
            <li>• Make sure you're logged in</li>
            <li>• Check your account permissions</li>
            <li>• Contact support if needed</li>
          </ul>
        </div>
      </div>
    </div>
  );
};
