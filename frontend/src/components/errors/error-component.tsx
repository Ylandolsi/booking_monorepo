import { Button } from '@/components/ui';
import { Home, ArrowLeft } from 'lucide-react';
import { AlertCircle, XCircle, CheckCircle, AlertTriangle, Info } from 'lucide-react';
import { routeBuilder, routes } from '@/config';
import { useAppNavigation } from '@/hooks';

interface ErrorComponenetProps {
  title?: string;
  message?: string;
  showHomeButton?: boolean;
  showBackButton?: boolean;
  iconType?: keyof typeof errorIconMap;
}

const errorIconMap = {
  default: AlertCircle,
  destructive: XCircle,
  success: CheckCircle,
  warning: AlertTriangle,
  info: Info,
} as const;

export const ErrorComponenet = ({
  title = 'Page Not Found',
  message = "The page you're looking for doesn't exist or has been moved.",
  showHomeButton = true,
  showBackButton = true,
  iconType = 'default',
}: ErrorComponenetProps) => {
  const IconComponent = errorIconMap[iconType] || errorIconMap.default;
  const navigate = useAppNavigation();

  const handleGoHome = () => {
    navigate.goToApp();
  };

  const handleGoBack = () => {
    window.history.back();
  };

  return (
    <div className="flex min-h-screen items-center justify-center p-4">
      <div className="w-full max-w-md space-y-6 text-center">
        {/* 404 Icon */}
        <div className="bg-primary/30 mx-auto flex h-16 w-16 items-center justify-center rounded-full">
          <IconComponent className="text-foreground h-6 w-6" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-foreground text-2xl font-bold">{title}</h1>
          <p className="text-muted-foreground">{message}</p>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col justify-center gap-3 sm:flex-row">
          {showBackButton && (
            <Button variant="outline" onClick={handleGoBack} className="flex items-center gap-2">
              <ArrowLeft className="h-4 w-4" />
              Go Back
            </Button>
          )}

          {showHomeButton && (
            <Button onClick={handleGoHome} className="flex items-center gap-2">
              <Home className="h-4 w-4" />
              Go Home
            </Button>
          )}
        </div>

        {/* Helpful Links */}
        <div className="text-muted-foreground flex flex-col items-center justify-center text-xs">
          <p>You might want to check these pages instead:</p>
          <div className="mt-2 space-y-1">
            <button onClick={() => navigate.goToApp()} className="text-primary block hover:underline">
              Dashboard
            </button>
            <button
              onClick={
                () =>
                  // TODO : fix this
                  navigate.goTo({ to: routes.to.app.root() }) // TODO
              }
              className="text-primary block hover:underline"
            >
              My Profile
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
