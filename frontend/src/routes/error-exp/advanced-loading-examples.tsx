import { createFileRoute } from '@tanstack/react-router';
// @ts-nocheck
import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  QueryWrapper,
  ProfileQueryWrapper,
  ListQueryWrapper,
  CardQueryWrapper,
} from '@/components/wrappers/query-wrapper';
import {
  LoadingState,
  PageLoading,
  ContentLoading,
  Skeleton,
  Spinner,
} from '@/components/ui';
import {
  MainErrorFallback,
  NetworkError,
  ErrorComponenet,
  Unauthorized,
} from '@/components/errors';
import { Button, Card, Badge } from '@/components/ui';
import { authQueryKeys } from '@/features/auth';

// ============================================================================
// MOCK API FUNCTIONS
// ============================================================================

const mockApi = {
  // User data
  getUser: async (id: string) => {
    await new Promise((resolve) => setTimeout(resolve, 2000));
    if (id === 'error') throw new Error('User not found');
    if (id === 'network') throw new Error('Network Error: Failed to fetch');
    return {
      id,
      name: 'John Doe',
      email: 'john@example.com',
      avatar: 'https://via.placeholder.com/150',
      bio: 'Software developer with 5 years of experience',
      followers: 1234,
      following: 567,
    };
  },

  // List data
  getUsers: async (page: number = 1) => {
    await new Promise((resolve) => setTimeout(resolve, 1500));
    if (page === 999) throw new Error('Network Error: Connection timeout');
    return Array.from({ length: 10 }, (_, i) => ({
      id: `${page}-${i}`,
      name: `User ${page}-${i}`,
      email: `user${page}-${i}@example.com`,
      status: i % 3 === 0 ? 'online' : 'offline',
    }));
  },

  // Card data
  getDashboardData: async () => {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    return {
      totalUsers: 12345,
      activeUsers: 8901,
      revenue: 45678,
      growth: 12.5,
    };
  },

  // Form submission
  submitForm: async (data: any) => {
    await new Promise((resolve) => setTimeout(resolve, 2000));
    if (data.email === 'error@example.com') throw new Error('Invalid email');
    return { success: true, id: Date.now() };
  },

  // File upload
  uploadFile: async (file: File) => {
    await new Promise((resolve) => setTimeout(resolve, 3000));
    if (file.name.includes('error')) throw new Error('File upload failed');
    return { url: 'https://example.com/file.jpg', size: file.size };
  },

  // Pagination
  getPaginatedData: async (page: number, limit: number = 10) => {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    if (page > 5) return { data: [], hasMore: false };
    return {
      data: Array.from({ length: limit }, (_, i) => ({
        id: `${page}-${i}`,
        title: `Item ${page}-${i}`,
        description: `Description for item ${page}-${i}`,
      })),
      hasMore: page < 5,
      total: 50,
    };
  },

  // Infinite scroll
  getInfiniteData: async ({ pageParam = 0 }) => {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    return {
      data: Array.from({ length: 10 }, (_, i) => ({
        id: pageParam + i,
        title: `Post ${pageParam + i}`,
        content: `Content for post ${pageParam + i}`,
      })),
      nextCursor: pageParam + 10,
      hasMore: pageParam < 50,
    };
  },
};

// ============================================================================
// EXAMPLE 1: BASIC LOADING STATES
// ============================================================================

export const BasicLoadingExamples = () => {
  const [loadingType, setLoadingType] = useState<'spinner' | 'dots' | 'pulse'>(
    'spinner',
  );

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Basic Loading States</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setLoadingType('spinner')}>Spinner</Button>
        <Button onClick={() => setLoadingType('dots')}>Dots</Button>
        <Button onClick={() => setLoadingType('pulse')}>Pulse</Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="p-4 border rounded-lg">
          <h4 className="font-medium mb-2">Loading State</h4>
          <LoadingState type={loadingType} message="Loading content..." />
        </div>

        <div className="p-4 border rounded-lg">
          <h4 className="font-medium mb-2">Page Loading</h4>
          <PageLoading />
        </div>

        <div className="p-4 border rounded-lg">
          <h4 className="font-medium mb-2">Content Loading</h4>
          <ContentLoading />
        </div>
      </div>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 2: PROFILE LOADING WITH QUERY WRAPPER
// ============================================================================

export const ProfileLoadingExample = () => {
  const [userId, setUserId] = useState('1');
  const userQuery = useQuery({
    queryKey: ['user', userId],
    queryFn: () => mockApi.getUser(userId),
    enabled: !!userId,
  });

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Profile Loading Example</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setUserId('1')}>Load User 1</Button>
        <Button onClick={() => setUserId('error')}>Load Error</Button>
        <Button onClick={() => setUserId('network')}>Load Network Error</Button>
      </div>

      <ProfileQueryWrapper query={userQuery}>
        {(user) => (
          <div className="space-y-4">
            <div className="flex items-center gap-4">
              <img
                src={user.avatar}
                alt={user.name}
                className="w-16 h-16 rounded-full"
              />
              <div>
                <h4 className="font-semibold">{user.name}</h4>
                <p className="text-muted-foreground">{user.email}</p>
              </div>
            </div>
            <p>{user.bio}</p>
            <div className="flex gap-4">
              <span>{user.followers} followers</span>
              <span>{user.following} following</span>
            </div>
          </div>
        )}
      </ProfileQueryWrapper>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 3: LIST LOADING WITH PAGINATION
// ============================================================================

export const ListLoadingExample = () => {
  const [page, setPage] = useState(1);
  const usersQuery = useQuery({
    queryKey: ['users', page],
    queryFn: () => mockApi.getUsers(page),
    keepPreviousData: true,
  });

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">List Loading Example</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setPage(1)}>Page 1</Button>
        <Button onClick={() => setPage(2)}>Page 2</Button>
        <Button onClick={() => setPage(999)}>Error Page</Button>
      </div>

      <ListQueryWrapper query={usersQuery} count={5}>
        {(users) => (
          <div className="space-y-2">
            {users.map((user) => (
              <div
                key={user.id}
                className="flex items-center justify-between p-3 border rounded"
              >
                <div>
                  <h4 className="font-medium">{user.name}</h4>
                  <p className="text-sm text-muted-foreground">{user.email}</p>
                </div>
                <Badge
                  variant={user.status === 'online' ? 'default' : 'secondary'}
                >
                  {user.status}
                </Badge>
              </div>
            ))}
          </div>
        )}
      </ListQueryWrapper>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 4: CARD LOADING WITH DASHBOARD DATA
// ============================================================================

export const CardLoadingExample = () => {
  const dashboardQuery = useQuery({
    queryKey: ['dashboard'],
    queryFn: mockApi.getDashboardData,
  });

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Card Loading Example</h3>

      <CardQueryWrapper query={dashboardQuery}>
        {(data) => (
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium">Total Users</h4>
              <p className="text-2xl font-bold">
                {data.totalUsers.toLocaleString()}
              </p>
            </div>
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium">Active Users</h4>
              <p className="text-2xl font-bold">
                {data.activeUsers.toLocaleString()}
              </p>
            </div>
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium">Revenue</h4>
              <p className="text-2xl font-bold">
                ${data.revenue.toLocaleString()}
              </p>
            </div>
            <div className="p-4 border rounded-lg">
              <h4 className="font-medium">Growth</h4>
              <p className="text-2xl font-bold text-green-600">
                +{data.growth}%
              </p>
            </div>
          </div>
        )}
      </CardQueryWrapper>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 5: FORM SUBMISSION WITH LOADING STATE
// ============================================================================

export const FormSubmissionExample = () => {
  const [formData, setFormData] = useState({ name: '', email: '' });
  const queryClient = useQueryClient();

  const submitMutation = useMutation({
    mutationFn: mockApi.submitForm,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: authQueryKeys.currentUser() });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    submitMutation.mutate(formData);
  };

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Form Submission Example</h3>

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-sm font-medium mb-1">Name</label>
          <input
            type="text"
            value={formData.name}
            onChange={(e) =>
              setFormData((prev) => ({ ...prev, name: e.target.value }))
            }
            className="w-full p-2 border rounded"
            required
          />
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Email</label>
          <input
            type="email"
            value={formData.email}
            onChange={(e) =>
              setFormData((prev) => ({ ...prev, email: e.target.value }))
            }
            className="w-full p-2 border rounded"
            required
          />
        </div>

        <Button
          type="submit"
          isLoading={submitMutation.isPending}
          disabled={submitMutation.isPending}
        >
          {submitMutation.isPending ? 'Submitting...' : 'Submit'}
        </Button>
      </form>

      {submitMutation.isSuccess && (
        <div className="mt-4 p-3 bg-green-100 border border-green-400 rounded">
          Form submitted successfully!
        </div>
      )}

      {submitMutation.isError && (
        <div className="mt-4 p-3 bg-red-100 border border-red-400 rounded">
          Error: {submitMutation.error?.message}
        </div>
      )}
    </Card>
  );
};

// ============================================================================
// EXAMPLE 6: FILE UPLOAD WITH PROGRESS
// ============================================================================

export const FileUploadExample = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadProgress, setUploadProgress] = useState(0);

  const uploadMutation = useMutation({
    mutationFn: mockApi.uploadFile,
    onMutate: () => {
      setUploadProgress(0);
      const interval = setInterval(() => {
        setUploadProgress((prev) => {
          if (prev >= 90) {
            clearInterval(interval);
            return 90;
          }
          return prev + 10;
        });
      }, 300);
    },
    onSuccess: () => {
      setUploadProgress(100);
      setTimeout(() => setUploadProgress(0), 1000);
    },
  });

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) setSelectedFile(file);
  };

  const handleUpload = () => {
    if (selectedFile) {
      uploadMutation.mutate(selectedFile);
    }
  };

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">File Upload Example</h3>

      <div className="space-y-4">
        <input
          type="file"
          onChange={handleFileChange}
          className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
        />

        {selectedFile && (
          <div className="p-3 bg-gray-50 rounded">
            <p className="text-sm">Selected: {selectedFile.name}</p>
            <p className="text-xs text-gray-500">
              Size: {(selectedFile.size / 1024).toFixed(2)} KB
            </p>
          </div>
        )}

        <Button
          onClick={handleUpload}
          isLoading={uploadMutation.isPending}
          disabled={!selectedFile || uploadMutation.isPending}
        >
          {uploadMutation.isPending ? 'Uploading...' : 'Upload File'}
        </Button>

        {uploadProgress > 0 && (
          <div className="w-full bg-gray-200 rounded-full h-2">
            <div
              className="bg-blue-600 h-2 rounded-full transition-all duration-300"
              style={{ width: `${uploadProgress}%` }}
            />
          </div>
        )}

        {uploadMutation.isSuccess && (
          <div className="p-3 bg-green-100 border border-green-400 rounded">
            File uploaded successfully!
          </div>
        )}

        {uploadMutation.isError && (
          <div className="p-3 bg-red-100 border border-red-400 rounded">
            Upload failed: {uploadMutation.error?.message}
          </div>
        )}
      </div>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 7: INFINITE SCROLL WITH LOADING
// ============================================================================

export const InfiniteScrollExample = () => {
  const [items, setItems] = useState<any[]>([]);
  const [hasMore, setHasMore] = useState(true);
  const [page, setPage] = useState(0);

  const loadMoreQuery = useQuery({
    queryKey: ['infinite', page],
    queryFn: () => mockApi.getInfiniteData({ pageParam: page }),
    enabled: hasMore,
    onSuccess: (data) => {
      setItems((prev) => [...prev, ...data.data]);
      setHasMore(data.hasMore);
    },
  });

  const loadMore = () => {
    if (hasMore && !loadMoreQuery.isLoading) {
      setPage((prev) => prev + 10);
    }
  };

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Infinite Scroll Example</h3>

      <div className="space-y-4 max-h-96 overflow-y-auto">
        {items.map((item) => (
          <div key={item.id} className="p-3 border rounded">
            <h4 className="font-medium">{item.title}</h4>
            <p className="text-sm text-muted-foreground">{item.content}</p>
          </div>
        ))}

        {loadMoreQuery.isLoading && (
          <div className="flex justify-center p-4">
            <LoadingState type="dots" message="Loading more..." />
          </div>
        )}

        {hasMore && !loadMoreQuery.isLoading && (
          <Button onClick={loadMore} className="w-full">
            Load More
          </Button>
        )}
      </div>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 8: CONDITIONAL LOADING STATES
// ============================================================================

export const ConditionalLoadingExample = () => {
  const [scenario, setScenario] = useState<
    'loading' | 'error' | 'success' | 'empty'
  >('loading');
  const [loadingType, setLoadingType] = useState<
    'skeleton' | 'spinner' | 'custom'
  >('skeleton');

  const mockQuery = useQuery({
    queryKey: ['conditional', scenario],
    queryFn: async () => {
      await new Promise((resolve) => setTimeout(resolve, 2000));

      switch (scenario) {
        case 'error':
          throw new Error('Something went wrong');
        case 'empty':
          return [];
        case 'success':
          return { data: 'Success data' };
        default:
          return new Promise(() => {}); // Never resolves
      }
    },
    enabled: scenario === 'loading',
  });

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Conditional Loading States</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setScenario('loading')}>Loading</Button>
        <Button onClick={() => setScenario('error')}>Error</Button>
        <Button onClick={() => setScenario('success')}>Success</Button>
        <Button onClick={() => setScenario('empty')}>Empty</Button>
      </div>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setLoadingType('skeleton')}>Skeleton</Button>
        <Button onClick={() => setLoadingType('spinner')}>Spinner</Button>
        <Button onClick={() => setLoadingType('custom')}>Custom</Button>
      </div>

      <QueryWrapper
        query={mockQuery}
        skeletonType={loadingType}
        errorFallback={(error) => (
          <div className="p-4 border border-red-300 bg-red-50 rounded">
            <h4 className="font-medium text-red-800">Custom Error Handler</h4>
            <p className="text-sm text-red-600">{error.message}</p>
          </div>
        )}
      >
        {(data) => (
          <div className="p-4 border border-green-300 bg-green-50 rounded">
            <h4 className="font-medium text-green-800">Success!</h4>
            <p className="text-sm text-green-600">{JSON.stringify(data)}</p>
          </div>
        )}
      </QueryWrapper>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 9: ERROR BOUNDARY PATTERNS
// ============================================================================

export const ErrorBoundaryExample = () => {
  const [errorType, setErrorType] = useState<
    'network' | 'not-found' | 'unauthorized' | 'general'
  >('network');

  const renderError = () => {
    switch (errorType) {
      case 'network':
        return <NetworkError onRetry={() => console.log('Retrying...')} />;
      case 'not-found':
        return (
          <ErrorComponenet
            title="Custom 404"
            message="This page doesn't exist"
          />
        );
      case 'unauthorized':
        return (
          <Unauthorized title="Access Denied" message="You need to log in" />
        );
      default:
        return <MainErrorFallback error={new Error('General error')} />;
    }
  };

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Error Boundary Patterns</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setErrorType('network')}>Network Error</Button>
        <Button onClick={() => setErrorType('not-found')}>Not Found</Button>
        <Button onClick={() => setErrorType('unauthorized')}>
          Unauthorized
        </Button>
        <Button onClick={() => setErrorType('general')}>General Error</Button>
      </div>

      <div className="border rounded-lg">{renderError()}</div>
    </Card>
  );
};

// ============================================================================
// EXAMPLE 10: COMPLEX LOADING SCENARIOS
// ============================================================================

export const ComplexLoadingExample = () => {
  const [scenario, setScenario] = useState<
    'multi-step' | 'parallel' | 'sequential'
  >('multi-step');
  const [step, setStep] = useState(1);

  // Multi-step loading
  const step1Query = useQuery({
    queryKey: ['step1'],
    queryFn: () =>
      new Promise((resolve) =>
        setTimeout(() => resolve('Step 1 complete'), 1000),
      ),
    enabled: scenario === 'multi-step' && step >= 1,
  });

  const step2Query = useQuery({
    queryKey: ['step2'],
    queryFn: () =>
      new Promise((resolve) =>
        setTimeout(() => resolve('Step 2 complete'), 1500),
      ),
    enabled: scenario === 'multi-step' && step >= 2,
  });

  const step3Query = useQuery({
    queryKey: ['step3'],
    queryFn: () =>
      new Promise((resolve) =>
        setTimeout(() => resolve('Step 3 complete'), 2000),
      ),
    enabled: scenario === 'multi-step' && step >= 3,
  });

  // Parallel loading
  const parallelQuery1 = useQuery({
    queryKey: ['parallel1'],
    queryFn: () =>
      new Promise((resolve) => setTimeout(() => resolve('Data 1'), 1000)),
    enabled: scenario === 'parallel',
  });

  const parallelQuery2 = useQuery({
    queryKey: ['parallel2'],
    queryFn: () =>
      new Promise((resolve) => setTimeout(() => resolve('Data 2'), 2000)),
    enabled: scenario === 'parallel',
  });

  const parallelQuery3 = useQuery({
    queryKey: ['parallel3'],
    queryFn: () =>
      new Promise((resolve) => setTimeout(() => resolve('Data 3'), 3000)),
    enabled: scenario === 'parallel',
  });

  return (
    <Card className="p-6">
      <h3 className="text-lg font-semibold mb-4">Complex Loading Scenarios</h3>

      <div className="flex gap-2 mb-4">
        <Button onClick={() => setScenario('multi-step')}>Multi-Step</Button>
        <Button onClick={() => setScenario('parallel')}>Parallel</Button>
        <Button onClick={() => setScenario('sequential')}>Sequential</Button>
      </div>

      {scenario === 'multi-step' && (
        <div className="space-y-4">
          <div className="flex gap-2">
            <Button onClick={() => setStep(1)}>Step 1</Button>
            <Button onClick={() => setStep(2)}>Step 2</Button>
            <Button onClick={() => setStep(3)}>Step 3</Button>
          </div>

          <div className="space-y-2">
            <div className="flex items-center gap-2">
              <Spinner size="sm" />
              <span>
                Step 1: {step1Query.isLoading ? 'Loading...' : step1Query.data}
              </span>
            </div>

            {step >= 2 && (
              <div className="flex items-center gap-2">
                <Spinner size="sm" />
                <span>
                  Step 2:{' '}
                  {step2Query.isLoading ? 'Loading...' : step2Query.data}
                </span>
              </div>
            )}

            {step >= 3 && (
              <div className="flex items-center gap-2">
                <Spinner size="sm" />
                <span>
                  Step 3:{' '}
                  {step3Query.isLoading ? 'Loading...' : step3Query.data}
                </span>
              </div>
            )}
          </div>
        </div>
      )}

      {scenario === 'parallel' && (
        <div className="space-y-2">
          <div className="flex items-center gap-2">
            <Spinner size="sm" />
            <span>
              Query 1:{' '}
              {parallelQuery1.isLoading ? 'Loading...' : parallelQuery1.data}
            </span>
          </div>
          <div className="flex items-center gap-2">
            <Spinner size="sm" />
            <span>
              Query 2:{' '}
              {parallelQuery2.isLoading ? 'Loading...' : parallelQuery2.data}
            </span>
          </div>
          <div className="flex items-center gap-2">
            <Spinner size="sm" />
            <span>
              Query 3:{' '}
              {parallelQuery3.isLoading ? 'Loading...' : parallelQuery3.data}
            </span>
          </div>
        </div>
      )}

      {scenario === 'sequential' && (
        <div className="space-y-4">
          <p className="text-muted-foreground">
            Sequential loading would be implemented with dependent queries
          </p>
          <Skeleton className="w-full h-32" />
        </div>
      )}
    </Card>
  );
};

// ============================================================================
// MAIN COMPONENT
// ============================================================================

export const AdvancedLoadingExamples = () => {
  return (
    <div className="p-6 space-y-8">
      <div>
        <h1 className="text-3xl font-bold mb-2">
          Advanced Loading & Error Examples
        </h1>
        <p className="text-muted-foreground">
          Comprehensive examples covering all possible loading and error
          handling scenarios
        </p>
      </div>

      <div className="space-y-6">
        <BasicLoadingExamples />
        <ProfileLoadingExample />
        <ListLoadingExample />
        <CardLoadingExample />
        <FormSubmissionExample />
        <FileUploadExample />
        <InfiniteScrollExample />
        <ConditionalLoadingExample />
        <ErrorBoundaryExample />
        <ComplexLoadingExample />
      </div>
    </div>
  );
};

export const Route = createFileRoute('/error-exp/advanced-loading-examples')({
  component: AdvancedLoadingExamples,
});
