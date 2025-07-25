import { MainErrorFallback } from '@/components/errors/main';
import { Spinner } from '@/components/ui/spinner';
import { ProfileInfo } from './profile-info';
import { ProfileImage } from './profile-image';
import { ProfileActions } from './profile-actions';
import { useProfileHeader } from '../../hooks/use-profile-header';

export function MobileHeader() {
  const { currentUser, error, isLoading, isSlugCurrent } = useProfileHeader();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <Spinner />;

  return (
    <div className="bg-card rounded-2xl shadow-lg border border-border/50 p-6">
      <div className="space-y-6">
        <div className="flex justify-between items-start">
          <ProfileImage size="sm" />
          <ProfileActions
            isSlugCurrent={isSlugCurrent}
            variant="vertical"
            className="flex flex-col gap-2"
          />
        </div>

        <ProfileInfo currentUser={currentUser} titleSize="sm" />
      </div>
    </div>
  );
}
