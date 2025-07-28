import { createFileRoute } from '@tanstack/react-router';
import React from 'react';
import { useQuery } from '@tanstack/react-query';
import { QueryWrapper } from '@/components/wrappers/query-wrapper';
import { LoadingState, Skeleton } from '@/components/ui';

// Simple mock API
const mockApi = {
  getData: async () => {
    await new Promise((resolve) => setTimeout(resolve, 2000));
    return { message: 'Data loaded successfully!' };
  },
  getError: async () => {
    await new Promise((resolve) => setTimeout(resolve, 500));
    throw new Error('Simulated error');
  },
};

export const SimpleLoadingDemo = () => {
  const dataQuery = useQuery({
    queryKey: ['simple-data'],
    queryFn: mockApi.getData,
  });

  const errorQuery = useQuery({
    queryKey: ['simple-error'],
    queryFn: mockApi.getError,
    retry: false,
  });

  return (
    <div className="p-6 space-y-6">
      <h2 className="text-2xl font-bold">Simple Loading Demo</h2>

      <div className="space-y-4">
        <div>
          <h3 className="text-lg font-semibold mb-2">Basic Loading States</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium mb-2">Spinner</h4>
              <LoadingState type="spinner" message="Loading..." />
            </div>

            <div className="p-4 border rounded-lg">
              <h4 className="font-medium mb-2">Dots</h4>
              <LoadingState type="dots" />
            </div>

            <div className="p-4 border rounded-lg">
              <h4 className="font-medium mb-2">Pulse</h4>
              <LoadingState type="pulse" message="Processing..." />
            </div>
          </div>
        </div>

        <div>
          <h3 className="text-lg font-semibold mb-2">Query Wrapper Example</h3>
          <div className="p-4 border rounded-lg">
            <QueryWrapper query={dataQuery}>
              {(data) => (
                <div>
                  <h4 className="font-medium">Success!</h4>
                  <p className="text-muted-foreground">{data.message}</p>
                </div>
              )}
            </QueryWrapper>
          </div>
        </div>

        <div>
          <h3 className="text-lg font-semibold mb-2">Error Example</h3>
          <div className="p-4 border rounded-lg">
            <QueryWrapper query={errorQuery}>
              {() => <div>This won't render due to error</div>}
            </QueryWrapper>
          </div>
        </div>

        <div>
          <h3 className="text-lg font-semibold mb-2">Skeleton Examples</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium mb-2">Card Skeleton</h4>
              <Skeleton className="w-full h-32" />
            </div>

            <div className="p-4 border rounded-lg">
              <h4 className="font-medium mb-2">Text Skeleton</h4>
              <div className="space-y-2">
                <Skeleton className="h-4 w-[80%]" />
                <Skeleton className="h-4 w-[60%]" />
                <Skeleton className="h-4 w-[90%]" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export const Route = createFileRoute('/error-exp/simple-loading-demo')({
  component: SimpleLoadingDemo,
});
