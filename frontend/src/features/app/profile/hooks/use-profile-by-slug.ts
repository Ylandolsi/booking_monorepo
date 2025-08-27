import { useUser } from '@/features/auth';
import { useProfile } from '@/features/app/profile/api';
import { useRequiredParam } from '@/hooks';

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

  // const

  return {
    user: isSlugCurrent ? currentUser : user,
    error: error || userError,
    isLoading: isLoading || userLoading,
    isSlugCurrent,
  };
}
