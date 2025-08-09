/**
 * Hook for creating consistent query state objects
 */
export function useQueryState<T>(
  data: T | undefined,
  isLoading: boolean,
  isError: boolean,
  error: Error | null | undefined,
  refetch: () => void,
) {
  return {
    data,
    isLoading,
    isError,
    error: error || null,
    refetch,
  };
}
