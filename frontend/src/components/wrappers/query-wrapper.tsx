import type { UseQueryResult } from '@tanstack/react-query';
import React from 'react';
import { MainErrorFallback, NetworkError } from '@/components/errors';
import {
  Spinner,
  Skeleton,
  CardSkeleton,
  ListSkeleton,
  ProfileSkeleton,
} from '@/components/ui';

interface QueryWrapperProps<T> {
  query: UseQueryResult<T>;
  children: (data: T) => React.ReactNode;
  loadingFallback?: React.ReactNode;
  errorFallback?: (error: Error) => React.ReactNode;
  skeletonType?: 'spinner' | 'card' | 'list' | 'profile' | 'custom';
  skeletonProps?: {
    count?: number;
    rows?: number;
    columns?: number;
  };
}

export function QueryWrapper<T>({
  query,
  children,
  loadingFallback,
  errorFallback,
  skeletonType = 'spinner',
  skeletonProps = {},
}: QueryWrapperProps<T>) {
  // Handle loading state
  if (query.isLoading) {
    if (loadingFallback) {
      return <>{loadingFallback}</>;
    }

    switch (skeletonType) {
      case 'card':
        return <CardSkeleton />;
      case 'list':
        return <ListSkeleton count={skeletonProps.count} />;
      case 'profile':
        return <ProfileSkeleton />;
      case 'custom':
        return <Skeleton className="w-full h-32" />;
      default:
        return (
          <div className="flex items-center justify-center p-8">
            <Spinner />
            <span className="ml-2 text-sm text-muted-foreground">
              Loading...
            </span>
          </div>
        );
    }
  }

  // Handle error state
  if (query.error) {
    if (errorFallback) {
      return errorFallback(query.error as Error);
    }

    // Check if it's a network error
    const error = query.error as Error;
    if (error.message.includes('Network') || error.message.includes('fetch')) {
      return <NetworkError onRetry={() => query.refetch()} />;
    }

    return <MainErrorFallback error={error} />;
  }

  // Handle no data
  if (!query.data) {
    return (
      <div className="flex items-center justify-center p-8 text-center">
        <div className="space-y-2">
          <p className="text-muted-foreground">No data available</p>
          <button
            onClick={() => query.refetch()}
            className="text-sm text-primary hover:underline"
          >
            Try again
          </button>
        </div>
      </div>
    );
  }

  // Render children with data
  return <>{children(query.data)}</>;
}

// Specialized wrappers for common use cases
export function ProfileQueryWrapper<T>({
  query,
  children,
  ...props
}: QueryWrapperProps<T>) {
  return (
    <QueryWrapper query={query} skeletonType="profile" {...props}>
      {children}
    </QueryWrapper>
  );
}

export function ListQueryWrapper<T>({
  query,
  children,
  count = 5,
  ...props
}: QueryWrapperProps<T> & { count?: number }) {
  return (
    <QueryWrapper
      query={query}
      skeletonType="list"
      skeletonProps={{ count }}
      {...props}
    >
      {children}
    </QueryWrapper>
  );
}

export function CardQueryWrapper<T>({
  query,
  children,
  ...props
}: QueryWrapperProps<T>) {
  return (
    <QueryWrapper query={query} skeletonType="card" {...props}>
      {children}
    </QueryWrapper>
  );
}
