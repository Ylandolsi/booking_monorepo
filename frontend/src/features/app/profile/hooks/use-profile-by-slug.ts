import { useUser } from '@/api/auth';
import { useProfile } from '@/features/app/profile/api';
import { useMentor, useRequiredParam } from '@/hooks';
import { useMentorDetails } from '@/features/app/mentor';

export function useProfileBySlug() {
  const { data: currentUser, error, isLoading } = useUser();
  const userSlug = useRequiredParam('userSlug');

  const isSlugCurrent = userSlug === currentUser?.slug;

  const {
    data: user,
    error: userError,
    isLoading: userLoading,
  } = useProfile(userSlug!, {
    enabled: !isSlugCurrent,
  });

  const { data: mentorData } = useMentorDetails(user?.slug, {
    enabled: !isSlugCurrent,
  });

  return {
    user: isSlugCurrent ? currentUser : user,
    error: error || userError,
    isLoading: isLoading || userLoading,
    mentorData,
    isSlugCurrent,
  };
}
