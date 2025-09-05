import { Button } from '@/components/ui';
import { GraduationCap, UserPlus, Home, Plug } from 'lucide-react';
import { useAppNavigation } from '@/hooks';

interface IntegrationRequiredProps {
  title?: string;
  message?: string;
  showBecomeMentorButton?: boolean;
  showHomeButton?: boolean;
  actionDescription?: string;
}

export const IntegrationRequired = ({
  title = 'Integration Required',
  message = 'You need to become a mentor before you can access this feature.',
  showBecomeMentorButton = true,
  showHomeButton = true,
  actionDescription = 'This action requires mentor privileges. Please complete your mentor registration to continue.',
}: IntegrationRequiredProps) => {
  const navigate = useAppNavigation();

  const handleIntegration = () => {
    navigate.goToIntegrations();
  };

  const handleGoHome = () => {
    navigate.goToApp();
  };

  const handleContactSupport = () => {
    navigate.goToSupport();
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <div className="max-w-md w-full text-center space-y-6">
        {/* Mentor Required Icon */}
        <div className="mx-auto w-16 h-16 bg-blue-100 dark:bg-blue-900/20 rounded-full flex items-center justify-center">
          <Plug className="w-8 h-8 text-blue-600 dark:text-blue-400" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-2xl font-bold text-foreground">{title}</h1>
          <p className="text-muted-foreground">{message}</p>
          {actionDescription && (
            <p className="text-sm text-muted-foreground bg-muted p-3 rounded-lg">
              {actionDescription}
            </p>
          )}
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          {showBecomeMentorButton && (
            <Button
              onClick={handleIntegration}
              className="flex items-center gap-2"
            >
              <UserPlus className="w-4 h-4" />
              Integrate your account 
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

        {/* Help Section */}
        <div className="pt-4 border-t border-border">
          <p className="text-xs text-muted-foreground">
            Need help with integrations ?{' '}
            <button
              onClick={handleContactSupport}
              className="text-primary hover:underline"
            >
              Contact support
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};
