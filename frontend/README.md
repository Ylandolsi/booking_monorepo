# Frontend Patterns and Documentation

This document outlines common patterns and best practices for the frontend application, including error handling, loading states, routing, and component usage.

## Table of Contents

- [Error Handling Patterns](#error-handling-patterns)
- [Loading Handling Patterns](#loading-handling-patterns)
- [Search Params](#search-params)
- [Uncaught Errors and Not Found Pages](#uncaught-errors-and-not-found-pages)
- [TanStack Query Patterns](#tanstack-query-patterns)
- [Patterns](#patterns)
  - [Alert](#alert)
  - [Guards](#guards)
    - [AuthGuard](#authguard)
    - [StoreGuard](#storeguard)
  - [Navigation](#navigation)

## Error Handling Patterns

### Basic Error Components

```ts
// Network error
<NetworkError
  onRetry={() => refetch()}
  customMessage="Custom network error message"
/>

// Not found (404)
<ErrorComponenet
  title="Custom 404"
  message="Custom not found message"
  showHomeButton={true}
  showBackButton={true}
/>

// Unauthorized (401/403)
<Unauthorized
  title="Access Denied"
  message="Custom unauthorized message"
  showLoginButton={true}
  showHomeButton={true}
/>

// Custom usage with specific message
<MentorRequired
  title="Become a Mentor to Schedule Sessions"
  message="You need mentor privileges to create and manage mentoring sessions."
  actionDescription="To schedule sessions with students, please complete your mentor registration first."
/>
```

### Error Component Selection Logic

```ts
const getErrorComponent = (error: any) => {
  const status = error.status || error.response?.status;

  if (!status || status === 0) {
    return <NetworkError onRetry={() => dataQuery.refetch()} />;
  }

  if (status === 404) {
    return <ErrorComponenet title="Resource Not Found" message={error.message} />;
  }

  if (status === 401 || status === 403) {
    return <Unauthorized title="Access Denied" message={error.message} />;
  }

  return <MainErrorFallback
            error={error}
            customMessage="Custom error message"
            showHomeButton={true}
            showBackButton={true}
          />
};
```

## Loading Handling Patterns

### 1. Loading State Types

```typescript
import { LoadingState } from '@/components/ui';

// Spinner loading
<LoadingState type="spinner" message="Loading..." />

// Dots loading
<LoadingState type="dots" />

// Pulse loading
<LoadingState type="pulse" message="Processing..." />
```

### 2. Specialized Loading Components

```typescript
import { PageLoading, ContentLoading, CardSkelton, ListSkelton } from '@/components/ui';

// Basic skeleton
<Skeleton className="w-full h-32" />

// Full page loading
<PageLoading />

// Content area loading
<ContentLoading />

// Card loading
<CardSkelton />

// List loading with custom count
<ListSkelton count={5} />

// Table skeleton
<TableSkeleton rows={10} columns={4} />
```

### Usage Example

```typescript
function MyComponent() {
  const [isLoading, setIsLoading] = useState(false);

  if (isLoading) {
    return <LoadingState type="spinner" message="Loading data..." />;
  }

  return <div>Content loaded!</div>;
}
```

## Search Params

### Basic Usage

```ts
// To use search params, configure the route file:
const routeApi = getRouteApi(ROUTE_PATHS.AUTH.EMAIL_VERIFICATION);
const { redirectTo, token, email } = routeApi.useSearch();

// Route file configuration:
export const Route = createFileRoute(ROUTE_PATHS.AUTH.EMAIL_VERIFICATION)({
  component: VerificationEmailPage,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    token: search.token as string | undefined,
    email: search.email as string | undefined,
  }),
});
```

### Accessing Search Params

```ts
const search = useSearch({ strict: false });
const redirectTo = (search.redirectTo as string) || './app';
```

### Conditional Routing Based on Search Params

```ts
export const Route = createFileRoute(ROUTE_PATHS.AUTH.RESET_PASSWORD)({
  component: ResetPassword,
  validateSearch: (search) => ({
    redirectTo: search.redirectTo as string | undefined,
    email: search.email as string | undefined,
    token: search.token as string | undefined,
  }),
  beforeLoad: ({ search }) => {
    if (!search.email || !search.token) {
      throw redirect({
        to: routeBuilder.auth.login(),
      });
    }
  },
});
```

## Uncaught Errors and Not Found Pages

Configure the root route to handle uncaught errors and not found pages:

```tsx
export const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools />
    </>
  ),
  notFoundComponent: () => <ErrorComponenet />, // If no route matches, render this
  errorComponent: () => <MainErrorFallback />, // If there is an uncaught error
});
```

## TanStack Query Patterns

### Mutations

#### Creating a Session

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
```

#### Updating a Session

```ts
const updateSessionMutation = useUpdateSession();
```

### Queries

#### Mentor Details Query

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

### Combining Multiple Queries into One State

```tsx
export function useUserMentorData() {
  const isLoading = userQuery.isLoading || (userQuery.data && mentorQuery.isLoading);

  // Determine combined error state
  const error = userQuery.error || mentorQuery.error;
  const isError = userQuery.isError || mentorQuery.isError;
..
    return {
    // Data
    user: userQuery.data,
    mentor: mentorQuery.data,

    // Loading states
    isLoading,
    isUserLoading: userQuery.isLoading,
    isMentorLoading: mentorQuery.isLoading,

    // Error states
    isError,
    error,
    userError: userQuery.error,
    mentorError: mentorQuery.error,
    }
}
```

## Patterns

### Alert

The Alert component is used to display notifications, such as success or error messages after form submissions.

#### Icon Mapping

Define an icon map for different alert variants:

```javascript
const alertIconMap = {
    default: AlertCircle,
    destructive: XCircle,
    success: CheckCircle,
    warning: AlertTriangle,
    info: Info,
} as const;
```

#### Error Alert Example

Use this pattern to show an error alert when a mutation fails:

```jsx
{
  becomeMentorMutation.isError && (
    <Alert variant={'destructive'}>
      {React.createElement(alertIconMap['destructive'])}
      <AlertTitle>Error</AlertTitle>
      <AlertDescription>Failed to register as a mentor. Please try again.</AlertDescription>
    </Alert>
  );
}
```

#### Success Alert Example

Use this pattern to show a success alert when a mutation succeeds:

```jsx
{
  becomeMentorMutation.isSuccess && (
    <Alert variant={'success'}>
      {React.createElement(alertIconMap['success'])}
      <AlertTitle>Success</AlertTitle>
      <AlertDescription>Successfully registered as a mentor.</AlertDescription>
    </Alert>
  );
}
```

### Guards

#### AuthGuard

The `AuthGuard` component is a higher-order component that handles authentication and authorization for protected routes.

##### Behavior

- Shows a loading spinner while checking authentication status.
- Redirects authenticated users away from auth pages.
- Redirects unauthenticated users to login with return path.
- Renders children only when access conditions are met.

##### Usage Example

```tsx
import { AuthGuard } from '@/components/guards/auth-guard';

<AuthGuard requireAuth>
  <ProtectedComponent />
</AuthGuard>;
```

#### StoreGuard

The `StoreGuard` component is a higher-order component that enforces users to set up their store before proceeding.

##### Behavior

- Shows a loading spinner while checking if the store is set up.
- Redirects users without a store to the setup page.
- Renders children only when the store is set up.

##### Usage Example

```tsx
import { StoreGuard } from '@/components/guards/store-guard';

<StoreGuard>
  <ComponentRequiringStore />
</StoreGuard>;
```

### Navigation

```typescript
// Navigation Hook
const nav = useAppNavigation();
nav.goToLogin({ redirectTo: '/app' });

// Link Component
<Link to={routes.to.auth.login({ redirectTo: '/app' })} variant="primary">
  Login
</Link>
```
