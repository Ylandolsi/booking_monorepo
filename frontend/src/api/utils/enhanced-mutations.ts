import { toast } from 'sonner';

// TODO : review this : skip for now : maybe useful later

import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { useEffect, useState } from 'react';

interface NetworkAwareQueryOptions<TQueryFnData, TError, TData, TQueryKey extends readonly unknown[]>
  extends Omit<UseQueryOptions<TQueryFnData, TError, TData, TQueryKey>, 'onError'> {
  enableOfflineCache?: boolean;
  showOfflineMessage?: boolean;
  onError?: (error: TError) => void;
}

export function useNetworkAwareQuery<TQueryFnData, TError, TData, TQueryKey extends readonly unknown[]>(
  options: NetworkAwareQueryOptions<TQueryFnData, TError, TData, TQueryKey>,
) {
  const [isOnline, setIsOnline] = useState(navigator.onLine);

  const { enableOfflineCache = true, showOfflineMessage = true, onError, ...queryOptions } = options;

  useEffect(() => {
    const handleOnline = () => setIsOnline(true);
    const handleOffline = () => {
      setIsOnline(false);
      if (showOfflineMessage) {
        toast.warning('You are offline. Some features may not work properly.');
      }
    };

    window.addEventListener('online', handleOnline);
    window.addEventListener('offline', handleOffline);

    return () => {
      window.removeEventListener('online', handleOnline);
      window.removeEventListener('offline', handleOffline);
    };
  }, [showOfflineMessage]);

  const query = useQuery({
    ...queryOptions,
    enabled: enableOfflineCache ? queryOptions.enabled : queryOptions.enabled && isOnline,
    retry: (failureCount, error) => {
      // Don't retry if offline
      if (!navigator.onLine) return false;

      // Use custom retry logic if provided
      if (typeof queryOptions.retry === 'function') {
        return queryOptions.retry(failureCount, error);
      }

      // Default retry logic based on error type
      const classified = require('./error-handling').classifyError(error);
      return classified.isRetryable && failureCount < 3;
    },
  });

  // Handle errors separately since onError is not part of the query options
  if (query.error && onError) {
    onError(query.error);
  }

  return query;
}
