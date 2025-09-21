import { useUser } from '@/api/auth';
import { useMentorDetails } from '@/features/app/mentor/become/api';

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
