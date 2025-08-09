# Centralized Loading and Error Handling

This guide demonstrates how to use the centralized loading and error handling patterns in the application.

## Components Available

### 1. QueryStateWrapper

A general-purpose wrapper for handling query states with customizable loading and error handling.

### 2. ProfileStateWrapper

A specialized wrapper for profile/mentor pages that includes additional validation logic.

### 3. useQueryState Hook

A utility hook for creating consistent query state objects from various data sources.

## Basic Usage

### Using QueryStateWrapper

```tsx
import { QueryStateWrapper } from '@/components/wrappers';
import { useQuery } from '@tanstack/react-query';

function MyComponent() {
  const dataQuery = useQuery({
    queryKey: ['my-data'],
    queryFn: fetchMyData,
  });

  return (
    <QueryStateWrapper
      query={dataQuery}
      loadingMessage="Loading your data..."
      loadingType="spinner"
    >
      {(data) => (
        <div>
          <h1>Data loaded!</h1>
          <pre>{JSON.stringify(data, null, 2)}</pre>
        </div>
      )}
    </QueryStateWrapper>
  );
}
```

### Using ProfileStateWrapper for User/Mentor Data

```tsx
import { ProfileStateWrapper, useQueryState } from '@/components/wrappers';
import { useUserMentorData } from '@/features/mentor';

function MyProfilePage() {
  const combinedQuery = useUserMentorData();

  // Convert to compatible query state
  const queryState = useQueryState(
    { user: combinedQuery.user, mentor: combinedQuery.mentor },
    combinedQuery.isLoading || false,
    combinedQuery.isError,
    combinedQuery.error as Error | null,
    combinedQuery.refetch,
  );

  return (
    <ProfileStateWrapper
      query={queryState}
      requiresMentor={true}
      loadingMessage="Loading your profile..."
    >
      {({ user, mentor }) => (
        <div>
          <h1>Welcome {user?.name}!</h1>
          {mentor && <p>Mentor since: {mentor.createdAt}</p>}
        </div>
      )}
    </ProfileStateWrapper>
  );
}
```

## Advanced Customization

### Custom Loading States

```tsx
<QueryStateWrapper
  query={dataQuery}
  loadingType="dots"
  loadingMessage="Processing your request..."
  loadingFallback={<MyCustomLoader />}
>
  {(data) => <MyComponent data={data} />}
</QueryStateWrapper>
```

### Custom Error Handling

```tsx
<QueryStateWrapper
  query={dataQuery}
  errorFallback={(error) => (
    <div className="error-container">
      <h2>Oops! Something went wrong</h2>
      <p>{error.message}</p>
      <button onClick={() => dataQuery.refetch()}>Try Again</button>
    </div>
  )}
>
  {(data) => <MyComponent data={data} />}
</QueryStateWrapper>
```

### Container Customization

```tsx
<QueryStateWrapper
  query={dataQuery}
  containerClassName="max-w-6xl mx-auto p-8"
  minHeight="600px"
  showRetryButton={false}
>
  {(data) => <MyComponent data={data} />}
</QueryStateWrapper>
```

## Benefits

1. **Consistency**: All loading and error states look and behave the same across the app
2. **Reduced Boilerplate**: No need to write loading/error handling in every component
3. **Centralized Logic**: Easy to update loading/error behavior app-wide
4. **Type Safety**: Proper TypeScript support with generic types
5. **Customization**: Flexible enough to handle special cases when needed

## Migration Guide

### Before (Manual Handling)

```tsx
function MyComponent() {
  const { data, isLoading, isError, error, refetch } = useQuery(...);

  if (isLoading) {
    return (
      <div className="flex justify-center">
        <Spinner />
        <span>Loading...</span>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="error-state">
        <p>Error: {error.message}</p>
        <button onClick={refetch}>Try Again</button>
      </div>
    );
  }

  return <div>My content with {data}</div>;
}
```

### After (Centralized Handling)

```tsx
function MyComponent() {
  const dataQuery = useQuery(...);

  return (
    <QueryStateWrapper query={dataQuery}>
      {(data) => <div>My content with {data}</div>}
    </QueryStateWrapper>
  );
}
```

## Common Patterns

### Multiple Queries

```tsx
function MultiQueryComponent() {
  const userQuery = useUser();
  const mentorQuery = useMentorDetails(userQuery.data?.slug);

  // Create combined state
  const combinedState = useQueryState(
    { user: userQuery.data, mentor: mentorQuery.data },
    userQuery.isLoading || mentorQuery.isLoading,
    userQuery.isError || mentorQuery.isError,
    userQuery.error || mentorQuery.error,
    () => Promise.all([userQuery.refetch(), mentorQuery.refetch()]),
  );

  return (
    <QueryStateWrapper query={combinedState}>
      {({ user, mentor }) => (
        <div>
          <UserProfile user={user} />
          {mentor && <MentorProfile mentor={mentor} />}
        </div>
      )}
    </QueryStateWrapper>
  );
}
```

### Conditional Loading

```tsx
function ConditionalComponent({ userId }: { userId?: string }) {
  const userQuery = useQuery({
    queryKey: ['user', userId],
    queryFn: () => fetchUser(userId!),
    enabled: !!userId,
  });

  if (!userId) {
    return <div>Please select a user</div>;
  }

  return (
    <QueryStateWrapper query={userQuery} emptyStateMessage="User not found">
      {(user) => <UserCard user={user} />}
    </QueryStateWrapper>
  );
}
```
