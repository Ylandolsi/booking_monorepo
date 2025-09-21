/**
 * React Query Integration with Error Handling
 */

import { useMutation, useQueryClient, type UseMutationOptions } from '@tanstack/react-query';
import { useErrorHandler, NetworkUtils } from './error-handling';
import { toast } from 'sonner';

// ===== ENHANCED MUTATION HOOK =====

interface EnhancedMutationOptions<TData, TError, TVariables, TContext>
  extends Omit<UseMutationOptions<TData, TError, TVariables, TContext>, 'mutationFn'> {
  mutationFn: (variables: TVariables) => Promise<TData>;
  successMessage?: string;
  enableRetry?: boolean;
  maxRetries?: number;
  invalidateQueries?: string[][];
  context?: string;
}

export function useEnhancedMutation<TData, TError, TVariables, TContext = unknown>(
  options: EnhancedMutationOptions<TData, TError, TVariables, TContext>,
) {
  const { handleErrorWithRetry } = useErrorHandler();
  const queryClient = useQueryClient();

  const {
    mutationFn,
    successMessage,
    enableRetry = false,
    maxRetries = 3,
    invalidateQueries = [],
    context = 'Mutation',
    onSuccess,
    onError,
    ...mutationOptions
  } = options;

  return useMutation({
    ...mutationOptions,
    mutationFn: enableRetry ? (variables: TVariables) => NetworkUtils.withRetry(() => mutationFn(variables), maxRetries) : mutationFn,
    onSuccess: (data: TData, variables: TVariables, ctx: TContext) => {
      // Show success message
      if (successMessage) {
        toast.success(successMessage);
      }

      // Invalidate queries
      invalidateQueries.forEach((queryKey) => {
        queryClient.invalidateQueries({ queryKey });
      });

      // Call user-provided onSuccess
      onSuccess?.(data, variables, ctx);
    },
    onError: (error: TError, variables: TVariables, ctx: TContext | undefined) => {
      // Handle error with retry option if enabled
      if (enableRetry) {
        handleErrorWithRetry(error, () => mutationFn(variables), context);
      } else {
        handleErrorWithRetry(error, undefined, context);
      }

      // Call user-provided onError
      onError?.(error, variables, ctx);
    },
  });
}

// ===== NETWORK AWARE QUERY HOOK =====

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

// ===== FORM MUTATION WRAPPER =====

interface FormMutationOptions<TFormData, TResponse> {
  mutationFn: (data: TFormData) => Promise<TResponse>;
  validationFn?: (data: TFormData) => { valid: boolean; errors: string[] };
  successMessage?: string;
  invalidateQueries?: string[][];
  context?: string;
  onSuccess?: (data: TResponse, formData: TFormData) => void;
  onError?: (error: any, formData: TFormData) => void;
}

export function useFormMutation<TFormData, TResponse>(options: FormMutationOptions<TFormData, TResponse>) {
  const {
    mutationFn,
    validationFn,
    successMessage = 'Operation completed successfully!',
    invalidateQueries = [],
    context = 'Form',
    onSuccess,
    onError,
  } = options;

  return useEnhancedMutation({
    mutationFn: async (data: TFormData) => {
      // Client-side validation
      if (validationFn) {
        const validation = validationFn(data);
        if (!validation.valid) {
          throw new Error(validation.errors.join(', '));
        }
      }

      return mutationFn(data);
    },
    successMessage,
    enableRetry: true,
    maxRetries: 2,
    invalidateQueries,
    context,
    onSuccess: (response, formData) => {
      onSuccess?.(response, formData);
    },
    onError: (error, formData) => {
      onError?.(error, formData);
    },
  });
}

// ===== FILE UPLOAD MUTATION =====

interface FileUploadMutationOptions<TFormData extends { file?: File }, TResponse> {
  mutationFn: (data: TFormData) => Promise<TResponse>;
  fileValidation?: {
    maxSizeMB: number;
    allowedTypes: string[];
    required?: boolean;
  };
  successMessage?: string;
  invalidateQueries?: string[][];
  context?: string;
  onProgress?: (progress: number) => void;
  onSuccess?: (data: TResponse, formData: TFormData) => void;
  onError?: (error: any, formData: TFormData) => void;
}

export function useFileUploadMutation<TFormData extends { file?: File }, TResponse>(options: FileUploadMutationOptions<TFormData, TResponse>) {
  const { FileValidation } = require('./error-handling');

  const {
    mutationFn,
    fileValidation,
    successMessage = 'File uploaded successfully!',
    invalidateQueries = [],
    context = 'File Upload',
    onProgress,
    onSuccess,
    onError,
  } = options;

  return useEnhancedMutation({
    mutationFn: async (data: TFormData) => {
      // File validation
      if (data.file && fileValidation) {
        const validation = FileValidation.validateImage(data.file, fileValidation.maxSizeMB);
        if (!validation.valid) {
          throw new Error(validation.errors.join(', '));
        }

        const typeValidation = FileValidation.validateFileType(data.file, fileValidation.allowedTypes);
        if (!typeValidation.valid) {
          throw new Error(typeValidation.error);
        }
      } else if (fileValidation?.required && !data.file) {
        throw new Error('File is required');
      }

      // Simulate progress if callback provided
      if (onProgress) {
        onProgress(0);
        const progressInterval = setInterval(() => {
          // This is a mock progress - in real implementation you'd use XMLHttpRequest or similar
          onProgress(Math.min(90, Math.random() * 100));
        }, 100);

        try {
          const result = await mutationFn(data);
          clearInterval(progressInterval);
          onProgress(100);
          return result;
        } catch (error) {
          clearInterval(progressInterval);
          throw error;
        }
      }

      return mutationFn(data);
    },
    successMessage,
    enableRetry: false, // Don't retry file uploads
    invalidateQueries,
    context,
    onSuccess: (response, formData) => {
      onSuccess?.(response, formData);
    },
    onError: (error, formData) => {
      onError?.(error, formData);
    },
  });
}

// ===== OPTIMISTIC UPDATE MUTATION =====

interface OptimisticMutationOptions<TData, TVariables> {
  mutationFn: (data: TVariables) => Promise<TData>;
  queryKey: readonly unknown[];
  updateFn: (oldData: any, variables: TVariables) => any;
  successMessage?: string;
  context?: string;
}

export function useOptimisticMutation<TData, TVariables>(options: OptimisticMutationOptions<TData, TVariables>) {
  const queryClient = useQueryClient();
  const { handleError } = useErrorHandler();

  const { mutationFn, queryKey, updateFn, successMessage } = options;

  return useMutation({
    mutationFn,
    onMutate: async (variables: TVariables) => {
      // Cancel outgoing refetches
      await queryClient.cancelQueries({ queryKey });

      // Snapshot previous value
      const previousData = queryClient.getQueryData(queryKey);

      // Optimistically update
      queryClient.setQueryData(queryKey, (old: any) => updateFn(old, variables));

      return { previousData };
    },
    onSuccess: () => {
      if (successMessage) {
        toast.success(successMessage);
      }
    },
    onError: (error, _variables, context) => {
      // Rollback on error
      if (context?.previousData) {
        queryClient.setQueryData(queryKey, context.previousData);
      }

      handleError(error, 'Optimistic Update');
    },
    onSettled: () => {
      // Refetch to ensure we have the latest data
      queryClient.invalidateQueries({ queryKey });
    },
  });
}
