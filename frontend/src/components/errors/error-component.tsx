import { Button } from '@/components/ui';
import { Home, ArrowLeft } from 'lucide-react';
import {
  AlertCircle,
  XCircle,
  CheckCircle,
  AlertTriangle,
  Info,
} from 'lucide-react';
import { routeBuilder } from '@/config';
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
    <div className="min-h-screen flex items-center justify-center p-4">
      <div className="max-w-md w-full text-center space-y-6">
        {/* 404 Icon */}
        <div className="mx-auto w-16 h-16 bg-muted rounded-full flex items-center justify-center">
          <IconComponent className="w-6 h-6 text-muted-foreground" />
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
              onClick={() => navigate.goToApp()}
              className="block text-primary hover:underline"
            >
              Dashboard
            </button>
            <button
              onClick={() =>
                // TODO : fix this
                navigate.goTo(routeBuilder.profile.user('uhavetofixthisslug'))
              }
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
