import { useUser } from '@/features/auth';
import { useRequiredParam } from '@/hooks';
import { useProfile } from '@/features/profile';

export function useProfileBySlug() {
  const { data: currentUser, error, isLoading } = useUser();
  const userSlug = useRequiredParam('userSlug');

  const {
    data: user,
    error: userError,
    isLoading: userLoading,
  } = useProfile(userSlug!);

  const isSlugCurrent = user?.slug === currentUser?.slug;

  return {
    currentUser,
    user,
    error: error || userError,
    isLoading: isLoading || userLoading,
    isSlugCurrent,
  };
}
