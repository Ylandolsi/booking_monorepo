import { useQuery } from '@tanstack/react-query';
import { userInfo } from '../api/profile-api';

// Get user profile by ID
export function useProfile(userSlug: string) {
  return useQuery({
    queryKey: ['profile', userSlug],
    queryFn: () => userInfo(userSlug),
    enabled: !!userSlug,
  });
}
