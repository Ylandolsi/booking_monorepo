import { ErrorComponenet, LoadingState, Badge } from '@/components';
import { IntegrateGoogle, IntegrateKonnect } from '@/features/app/integrations';
import { useUser } from '@/features/auth';
import { Settings } from 'lucide-react';

export function IntegrationPage() {
  const { data: user, isLoading, error } = useUser();

  if (error) {
    return (
      <ErrorComponenet
        message="Failed to load the user"
        title="Failed to fetch user"
      />
    );
  }
  if (isLoading) {
    return <LoadingState type="dots" />;
  }

  // Calculate integration stats
  const totalIntegrations = 2;
  const connectedIntegrations = (user?.integratedWithGoogle ? 1 : 0) + (user?.konnectWalletId ? 1 : 0);
  return (
    <div className="mx-auto p-6 space-y-8 max-w-6xl">
 

      {/* Available Integrations */}
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-2xl font-semibold flex items-center gap-3">
              <Settings className="h-6 w-6 text-muted-foreground" />
              Available Integrations
            </h2>
            <p className="text-muted-foreground mt-1">Connect your tools to enhance your mentoring workflow</p>
          </div>
          <Badge variant="outline" className="hidden sm:flex">
            {connectedIntegrations} of {totalIntegrations} connected
          </Badge>
        </div>
        
        <div className="grid gap-6 lg:gap-8">
          <div className="group">
            <IntegrateGoogle user={user} />
          </div>
          <div className="group">
            <IntegrateKonnect user={user} />
          </div>
        </div>
      </div>

      
    </div>
  );
}
