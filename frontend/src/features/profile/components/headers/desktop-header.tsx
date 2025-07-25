import { MainErrorFallback } from '@/components/errors/main';
import { Spinner } from '@/components/ui/spinner';
import { ProfileActions } from './profile-actions';
import { ProfileInfo } from './profile-info';
import { useProfileHeader } from '../../hooks/use-profile-header';
import { ProfileImage } from './profile-image';

export function DesktopHeader() {
  const { currentUser, error, isLoading, isSlugCurrent } = useProfileHeader();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <Spinner />;

  return (
    <div className="bg-card rounded-2xl shadow-lg border border-border/50 p-8">
      <div className="flex justify-between items-start">
        <div className="flex items-center gap-6">
          <ProfileImage size="lg" />
          <ProfileInfo currentUser={currentUser} titleSize="lg" />
        </div>

        <ProfileActions isSlugCurrent={isSlugCurrent} variant="vertical" />
      </div>
    </div>
  );
}
