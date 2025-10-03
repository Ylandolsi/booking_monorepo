import { Button } from '@/components/ui';
import { UserPlus, Home, Plug } from 'lucide-react';
import { useAppNavigation } from '@/hooks';

interface IntegrationRequiredProps {
  title?: string;
  message?: string;
  showIntegrationPageButtton?: boolean;
  showHomeButton?: boolean;
  actionDescription?: string;
}

export const IntegrationRequired = ({
  title = 'Integration Required',
  message = 'You need to integrate your account before you can access this feature.',
  showIntegrationPageButtton = true,
  showHomeButton = true,
  actionDescription = '',
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
    <div className="flex min-h-screen items-center justify-center p-4">
      <div className="w-full max-w-md space-y-6 text-center">
        {/* Mentor Required Icon */}
        <div className="bg-primary/40 mx-auto flex h-16 w-16 items-center justify-center rounded-full">
          <Plug className="text-foreground h-8 w-8" />
        </div>

        {/* Error Message */}
        <div className="space-y-2">
          <h1 className="text-foreground text-2xl font-bold">{title}</h1>
          <p className="text-muted-foreground">{message}</p>
          {actionDescription && <p className="text-muted-foreground bg-muted rounded-lg p-3 text-sm">{actionDescription}</p>}
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col justify-center gap-3 sm:flex-row">
          {showIntegrationPageButtton && (
            <Button onClick={handleIntegration} className="flex items-center gap-2">
              <UserPlus className="h-4 w-4" />
              Integrate your account
            </Button>
          )}

          {showHomeButton && (
            <Button variant="outline" onClick={handleGoHome} className="flex items-center gap-2">
              <Home className="h-4 w-4" />
              Go Home
            </Button>
          )}
        </div>

        {/* Help Section */}
        <div className="border-border border-t pt-4">
          <p className="text-muted-foreground text-xs">
            Need help with integrations ?{' '}
            <button onClick={handleContactSupport} className="text-primary hover:underline">
              Contact support
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};
