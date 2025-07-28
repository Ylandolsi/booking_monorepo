// ============================================================================
// UTILITY FUNCTIONS
// ============================================================================

export const invalidateUserQueries = (queryClient: any, userSlug?: string) => {
  if (userSlug) {
    queryClient.invalidateQueries({ queryKey: userQueryKeys.detail(userSlug) });
    queryClient.invalidateQueries({
      queryKey: experienceQueryKeys.list(userSlug),
    });
    queryClient.invalidateQueries({
      queryKey: educationQueryKeys.list(userSlug),
    });
    queryClient.invalidateQueries({
      queryKey: expertiseQueryKeys.list(userSlug),
    });
    queryClient.invalidateQueries({
      queryKey: languageQueryKeys.list(userSlug),
    });
  } else {
    queryClient.invalidateQueries({ queryKey: userQueryKeys.all() });
    queryClient.invalidateQueries({ queryKey: experienceQueryKeys.all() });
    queryClient.invalidateQueries({ queryKey: educationQueryKeys.all() });
    queryClient.invalidateQueries({ queryKey: expertiseQueryKeys.all() });
    queryClient.invalidateQueries({ queryKey: languageQueryKeys.all() });
  }
};

export const invalidateAuthQueries = (queryClient: any) => {
  queryClient.invalidateQueries({ queryKey: authQueryKeys.all() });
  queryClient.invalidateQueries({ queryKey: userQueryKeys.current() });
};

export const clearAllQueries = (queryClient: any) => {};

const updateExperienceMutation = useMutation({
  ...updateExperienceMutationOptions('exp-123'),
  onSuccess: () => {
    invalidateUserQueries(queryClient, userSlug);
  },
});
