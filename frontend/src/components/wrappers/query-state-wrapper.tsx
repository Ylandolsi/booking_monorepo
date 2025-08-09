import React from 'react';
import { LoadingState } from '@/components/ui';
import { MainErrorFallback, NetworkError } from '@/components/errors';
import { AlertCircle } from 'lucide-react';

/**
 * Enhanced QueryWrapper with centralized state management for complex loading/error scenarios
 */
interface QueryStateWrapperProps<T> {
  query: {
    data?: T;
    isLoading: boolean;
    isError: boolean;
    error?: Error | null;
    refetch: () => void;
  };
  children: (data: T) => React.ReactNode;

  // Loading customization
  loadingType?: 'spinner' | 'skeleton' | 'dots' | 'pulse';
  loadingMessage?: string;
  loadingFallback?: React.ReactNode;

  // Error customization
  errorFallback?: (error: Error) => React.ReactNode;
  showRetryButton?: boolean;

  // Empty state customization
  showEmptyState?: boolean;
  emptyStateMessage?: string;
  emptyStateFallback?: React.ReactNode;

  // Container styling
  containerClassName?: string;
  minHeight?: string;
}

export function QueryStateWrapper<T>({
  query,
  children,
  loadingType = 'spinner',
  loadingMessage = 'Loading...',
  loadingFallback,
  errorFallback,
  showRetryButton = true,
  showEmptyState = true,
  emptyStateMessage = 'No data available',
  emptyStateFallback,
  containerClassName = 'container mx-auto py-10 px-4 max-w-4xl',
  minHeight = '400px',
}: QueryStateWrapperProps<T>) {
  // Handle loading state
  if (query.isLoading) {
    if (loadingFallback) {
      return <div className={containerClassName}>{loadingFallback}</div>;
    }

    return (
      <div className={containerClassName}>
        <div className="flex items-center justify-center" style={{ minHeight }}>
          <LoadingState type={loadingType} message={loadingMessage} />
        </div>
      </div>
    );
  }

  // Handle error state
  if (query.isError && query.error) {
    if (errorFallback) {
      return (
        <div className={containerClassName}>{errorFallback(query.error)}</div>
      );
    }

    const error = query.error;
    const isNetworkError =
      error.message.includes('Network') ||
      error.message.includes('fetch') ||
      error.message.includes('connection');

    return (
      <div className={containerClassName}>
        <div className="flex items-center justify-center" style={{ minHeight }}>
          {isNetworkError ? (
            <NetworkError
              onRetry={query.refetch}
              customMessage="Failed to load data. Please check your connection and try again."
            />
          ) : (
            <div className="text-center space-y-4">
              <MainErrorFallback
                error={error}
                customMessage="Something went wrong while loading the data."
                showHomeButton={false}
                showBackButton={false}
              />
              {showRetryButton && (
                <button
                  onClick={query.refetch}
                  className="mt-4 px-4 py-2 bg-primary text-primary-foreground rounded hover:bg-primary/90 transition-colors"
                >
                  Try Again
                </button>
              )}
            </div>
          )}
        </div>
      </div>
    );
  }

  // Handle empty state
  if (showEmptyState && !query.data) {
    if (emptyStateFallback) {
      return <div className={containerClassName}>{emptyStateFallback}</div>;
    }

    return (
      <div className={containerClassName}>
        <div className="flex items-center justify-center" style={{ minHeight }}>
          <div className="text-center space-y-4">
            <AlertCircle className="w-16 h-16 mx-auto text-gray-400" />
            <h2 className="text-xl font-bold text-gray-900">
              No Data Available
            </h2>
            <p className="text-gray-600">{emptyStateMessage}</p>
            {showRetryButton && (
              <button
                onClick={query.refetch}
                className="px-4 py-2 bg-primary text-primary-foreground rounded hover:bg-primary/90 transition-colors"
              >
                Refresh
              </button>
            )}
          </div>
        </div>
      </div>
    );
  }

  // Render success state
  return <>{children(query.data!)}</>;
}

/**
 * Specialized wrapper for profile/mentor pages
 */
export function ProfileStateWrapper<T extends { user?: any; mentor?: any }>({
  query,
  children,
  requiresMentor = false,
  ...props
}: QueryStateWrapperProps<T> & { requiresMentor?: boolean }) {
  return (
    <QueryStateWrapper
      query={query}
      loadingMessage="Loading your profile..."
      emptyStateMessage={
        requiresMentor ? 'No mentor profile found' : 'Profile not available'
      }
      {...props}
    >
      {(data) => {
        // Additional validation for mentor requirement
        if (requiresMentor && !data.mentor) {
          return (
            <div className="container mx-auto py-10 px-4 max-w-4xl">
              <div className="text-center space-y-4">
                <AlertCircle className="w-16 h-16 mx-auto text-yellow-600" />
                <h2 className="text-xl font-bold">No Mentor Profile Found</h2>
                <p className="text-gray-600">
                  It seems your mentor profile is not available.
                </p>
              </div>
            </div>
          );
        }

        return children(data);
      }}
    </QueryStateWrapper>
  );
}
