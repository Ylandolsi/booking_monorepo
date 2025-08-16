import { Spinner, Skeleton } from './index';
import { Loader2 } from 'lucide-react';

interface LoadingStateProps {
  type?: 'spinner' | 'skeleton' | 'dots' | 'pulse';
  size?: 'sm' | 'md' | 'lg' | 'xl';
  message?: string;
  className?: string;
}

export const LoadingState = ({
  type = 'spinner',
  size = 'md',
  message = 'Loading...',
  className = '',
}: LoadingStateProps) => {
  const renderLoading = () => {
    switch (type) {
      case 'skeleton':
        return <Skeleton className="w-full h-32" />;
      case 'dots':
        return (
          <div className="flex space-x-1">
            <div
              className="w-2 h-2 bg-current rounded-full animate-bounce"
              style={{ animationDelay: '0ms' }}
            />
            <div
              className="w-2 h-2 bg-current rounded-full animate-bounce"
              style={{ animationDelay: '150ms' }}
            />
            <div
              className="w-2 h-2 bg-current rounded-full animate-bounce"
              style={{ animationDelay: '300ms' }}
            />
          </div>
        );
      case 'pulse':
        return (
          <div className="flex items-center space-x-2">
            <Loader2 className="animate-spin" />
            <span className="text-sm text-muted-foreground">{message}</span>
          </div>
        );
      default:
        return <Spinner size={size} />;
    }
  };

  return (
    <div className={`flex items-center justify-center p-4 ${className}`}>
      <div className="flex flex-col items-center space-y-2">
        {renderLoading()}
        {type === 'spinner' && message && (
          <span className="text-sm text-muted-foreground">{message}</span>
        )}
      </div>
    </div>
  );
};

// Loader in the center of the page and height on the full screen
export const PageLoading = () => (
  <div className="min-h-screen flex items-center justify-center">
    <LoadingState type="spinner" size="lg" message="Loading page..." />
  </div>
);
export const PageLoading2 = ({
  title,
  description,
}: {
  title: string;
  description: string;
}) => (
  <div className="mx-auto">
    <div className="flex items-center justify-center min-h-[400px]">
      <div className="text-center space-y-4">
        <Spinner className="w-8 h-8 mx-auto" />
        <p className="text-lg text-gray-600">{title}...</p>
        <p className="text-sm text-gray-500">{description}...</p>
      </div>
    </div>
  </div>
);
// height = height of the content : we need to specify it
export const ContentLoading = () => (
  <div className="flex items-center justify-center p-8">
    <LoadingState type="spinner" size="md" message="Loading content..." />
  </div>
);

// can be used after clicking on load more !
export const DotsContent = () => {
  <div className="flex justify-center p-4">
    <LoadingState type="dots" message="Loading more..." />
  </div>;
};

// skelton of rectangle : width filling the parent !
export const CardSkelton = () => (
  <div className="p-6">
    <Skeleton className="w-full h-32" />
  </div>
);

export const ListSkelton = ({ count = 3 }: { count?: number }) => (
  <div className="space-y-3">
    {Array.from({ length: count }).map((_, i) => (
      <div key={i} className="flex items-center space-x-4 p-4">
        <Skeleton className="h-10 w-10 rounded-full" />
        <div className="space-y-2 flex-1">
          <Skeleton className="h-4 w-[80%]" />
          <Skeleton className="h-3 w-[60%]" />
        </div>
      </div>
    ))}
  </div>
);
