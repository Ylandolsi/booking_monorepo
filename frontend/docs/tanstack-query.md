### Mutation

```ts
export const createSession = async ({ data }: { data: CreateProductInput }) => {
  // Implementation here
};

export const useCreateSession = (options?: { onSuccess?: (data: PatchPostSessionResponse) => void; onError?: (error: Error) => void }) => {
  return useMutation<PatchPostSessionResponse, Error, { data: CreateProductInput }>({
    mutationFn: createSession,
    meta: {
      // invalidatesQuery: [SESSION_QUERY_KEY], // array of arrays: array of query keys
      successMessage: 'Session created successfully!',
      errorMessage: 'Failed to create session',
      onSuccess: options?.onSuccess,
      onError: options?.onError,
    },
  });
};

const updateSessionMutation = useUpdateSession();
```

### Query

```ts
// Query options for overrides:
export function useMentorDetails(userSlug?: string | null, overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Mentor, Error> {
  return useQuery(
    queryOptions({
      queryKey: mentorQueryKeys.mentorProfile(userSlug),
      queryFn: () => mentorDetails(userSlug),
      enabled: !!userSlug,
      ...overrides,
    }),
  );
}
```
