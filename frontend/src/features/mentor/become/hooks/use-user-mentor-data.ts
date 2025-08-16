import { useUser } from '@/features/auth';
import { useMentorDetails } from '@/features/mentor/become/api';

/**
 * Combined hook for user and mentor data with centralized loading/error handling
 * This hook automatically fetches mentor details when user data is available
 */
export function useUserMentorData() {
  const userQuery = useUser();
  const mentorQuery = useMentorDetails(userQuery.data?.slug, {
    enabled: !!userQuery.data?.slug,
  });

  // Determine combined loading state
  const isLoading =
    userQuery.isLoading || (userQuery.data && mentorQuery.isLoading);

  // Determine combined error state
  const error = userQuery.error || mentorQuery.error;
  const isError = userQuery.isError || mentorQuery.isError;

  // Combined refetch function
  const refetch = async () => {
    const promises = [];
    if (userQuery.isError) promises.push(userQuery.refetch());
    if (mentorQuery.isError) promises.push(mentorQuery.refetch());

    if (promises.length === 0) {
      // If no errors, refetch both
      promises.push(userQuery.refetch(), mentorQuery.refetch());
    }

    return Promise.all(promises);
  };

  return {
    // Data
    user: userQuery.data,
    mentor: mentorQuery.data,

    // Loading states
    isLoading,
    isUserLoading: userQuery.isLoading,
    isMentorLoading: mentorQuery.isLoading,

    // Error states
    isError,
    error,
    userError: userQuery.error,
    mentorError: mentorQuery.error,

    // Actions
    refetch,
    refetchUser: userQuery.refetch,
    refetchMentor: mentorQuery.refetch,

    // Individual query objects for advanced usage
    userQuery,
    mentorQuery,
  };
}
