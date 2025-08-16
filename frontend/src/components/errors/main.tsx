import { Button } from '@/components/ui';
import { AlertCircle, RefreshCw, Home, ArrowLeft } from 'lucide-react';
import { useNavigate } from '@tanstack/react-router';
import { routeBuilder } from '@/config';

interface ErrorFallbackProps {
  error?: Error;
  resetErrorBoundary?: () => void;
  showHomeButton?: boolean;
  showBackButton?: boolean;
  customMessage?: string;
}

export const MainErrorFallback = ({
  error,
  resetErrorBoundary,
  showHomeButton = true,
  showBackButton = true,
  customMessage,
}: ErrorFallbackProps) => {
  const navigate = useNavigate();

  const handleRefresh = () => {
    if (resetErrorBoundary) {
      resetErrorBoundary();
    } else {
      window.location.reload();
    }
  };

  const handleGoHome = () => {
    navigate({ to: routeBuilder.app.root() });
  };

  const handleGoBack = () => {
    window.history.back();
  };

  return (
    // <div className="min-h-screen flex items-center justify-center bg-background p-4">
    <div className="fixed inset-0 overflow-hidden p-4 flex min-h-screen items-center justify-center z-[51] bg-background">
      <div className="max-w-md w-full text-center space-y-6">
        {/* Error Icon */}
        <div className="mx-auto w-16 h-16 bg-destructive/10 rounded-full flex items-center justify-center">
          <AlertCircle className="w-8 h-8 text-destructive" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-2xl font-bold text-foreground">
            Oops! Something went wrong
          </h1>
          <p className="text-muted-foreground">
            {customMessage ||
              'We encountered an unexpected error. Please try again or contact support if the problem persists.'}
          </p>
          {error && process.env.NODE_ENV === 'development' && (
            <details className="mt-4 text-left">
              <summary className="cursor-pointer text-sm text-muted-foreground">
                Error Details (Development)
              </summary>
              <pre className="mt-2 text-xs bg-muted p-2 rounded overflow-auto">
                {error.message}
                {error.stack && `\n\n${error.stack}`}
              </pre>
            </details>
          )}
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          <Button onClick={handleRefresh} className="flex items-center gap-2">
            <RefreshCw className="w-4 h-4" />
            Try Again
          </Button>

          {showBackButton && (
            <Button
              variant="outline"
              onClick={handleGoBack}
              className="flex items-center gap-2"
            >
              <ArrowLeft className="w-4 h-4" />
              Go Back
            </Button>
          )}

          {showHomeButton && (
            <Button
              variant="outline"
              onClick={handleGoHome}
              className="flex items-center gap-2"
            >
              <Home className="w-4 h-4" />
              Go Home
            </Button>
          )}
        </div>

        {/* Support Info */}
        <div className="text-xs text-muted-foreground">
          <p>If this problem continues, please contact our support team.</p>
          <p className="mt-1">Error ID: {Date.now().toString(36)}</p>
        </div>
      </div>
    </div>
  );
};
