import { MainErrorFallback } from '@/components/errors/main';
import { ContentLoading } from '@/components/ui';
import { ProfileActions } from './profile-actions';
import { ProfileInfo } from './profile-info';
import { ProfileImage } from './profile-image';
import { useProfileBySlug } from '@/features/profile/hooks';

export function DesktopHeader() {
  const { error, isLoading } = useProfileBySlug();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <ContentLoading />;

  return (
    <div className="bg-card rounded-2xl shadow-lg border border-border/50 p-8">
      <div className="flex justify-between items-start">
        <div className="flex items-center gap-6">
          <ProfileImage size="lg" />
          <ProfileInfo titleSize="lg" />
        </div>

        <ProfileActions variant="vertical" />
      </div>
    </div>
  );
}
