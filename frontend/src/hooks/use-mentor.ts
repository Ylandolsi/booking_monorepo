import { useUser } from '@/features/auth';
import { useMentorDetails } from '@/features/mentor/become/api';

export const useMentor = () => {
  const { data: user } = useUser();
  const { data: mentor, isLoading } = useMentorDetails(user?.slug, {
    enabled: !!user,
  });

  return {
    isMentor: !!mentor,
    isLoading,
    user,
    mentor,
  };
};
