import { ErrorComponenet, LoadingState } from '@/components';
import { IntegrateGoogle, IntegrateKonnect } from '@/features/app/integrations';
import { useUser } from '@/features/auth';

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

  return (
    <>
      <div className="font-bold text-2xl pb-10">Integrations</div>
      <div className="flex flex-col gap-7">
        <IntegrateGoogle user={user} />
        <IntegrateKonnect user={user} />
      </div>
    </>
  );
}
