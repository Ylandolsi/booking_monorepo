import { MainErrorFallback } from '@/components/errors/main';
import { ContentLoading } from '@/components';
import { ProfileInfo } from './profile-info';
import { ProfileActions } from './profile-actions';
import { ProfileImage, useProfileBySlug } from '@/features/profile';

export function MobileHeader() {
  const { error, isLoading } = useProfileBySlug();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <ContentLoading />;

  return (
    <div className="bg-card rounded-2xl shadow-lg border border-border/50 p-6">
      <div className="space-y-6 ">
        <div className="flex justify-between items-center">
          <ProfileImage size="sm" />
          <ProfileActions variant="vertical" className="flex flex-col gap-2" />
        </div>

        <ProfileInfo titleSize="sm" />
      </div>
    </div>
  );
}
