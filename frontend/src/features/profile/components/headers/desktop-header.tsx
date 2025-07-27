import { MainErrorFallback } from '@/components/errors/main';
import { Spinner } from '@/components/ui';
import { ProfileActions } from './profile-actions';
import { ProfileInfo } from './profile-info';
import { ProfileImage } from './profile-image';
import { useProfileBySlug } from '@/features/profile/hooks';

export function DesktopHeader() {
  const { user, error, isLoading, isSlugCurrent } = useProfileBySlug();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <Spinner />;

  return (
    <div className="bg-card rounded-2xl shadow-lg border border-border/50 p-8">
      <div className="flex justify-between items-start">
        <div className="flex items-center gap-6">
          <ProfileImage size="lg" isCurrentUser={isSlugCurrent} />
          <ProfileInfo user={user} titleSize="lg" />
        </div>

        <ProfileActions isSlugCurrent={isSlugCurrent} variant="vertical" />
      </div>
    </div>
  );
}
