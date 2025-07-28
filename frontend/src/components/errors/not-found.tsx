import { Button } from '@/components/ui';
import { Search, Home, ArrowLeft } from 'lucide-react';
import { useNavigate } from '@tanstack/react-router';
import { paths } from '@/config';

interface NotFoundProps {
  title?: string;
  message?: string;
  showHomeButton?: boolean;
  showBackButton?: boolean;
}

export const NotFound = ({
  title = 'Page Not Found',
  message = "The page you're looking for doesn't exist or has been moved.",
  showHomeButton = true,
  showBackButton = true,
}: NotFoundProps) => {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate({ to: paths.app.root.getHref() });
  };

  const handleGoBack = () => {
    window.history.back();
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <div className="max-w-md w-full text-center space-y-6">
        {/* 404 Icon */}
        <div className="mx-auto w-16 h-16 bg-muted rounded-full flex items-center justify-center">
          <Search className="w-8 h-8 text-muted-foreground" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-2xl font-bold text-foreground">{title}</h1>
          <p className="text-muted-foreground">{message}</p>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
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
            <Button onClick={handleGoHome} className="flex items-center gap-2">
              <Home className="w-4 h-4" />
              Go Home
            </Button>
          )}
        </div>

        {/* Helpful Links */}
        <div className="text-xs text-muted-foreground flex flex-col justify-center items-center">
          <p>You might want to check these pages instead:</p>
          <div className="mt-2 space-y-1">
            <button
              onClick={() => navigate({ to: paths.app.root.getHref() })}
              className="block text-primary hover:underline"
            >
              Dashboard
            </button>
            <button
              onClick={() => navigate({ to: '/profile/me' })}
              className="block text-primary hover:underline"
            >
              My Profile
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
