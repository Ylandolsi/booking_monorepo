## 🎯 **Basic Loading States**

````typescript
export const useUserData = (userSlug?: string) => {
    const { data: user, isLoading: userLoading } = useQuery(getUserQueryOptions(userSlug));
    const { data: experiences, isLoading: experiencesLoading } = useQuery(getUserExperiencesQueryOptions(userSlug));
    const { data: educations, isLoading: educationsLoading } = useQuery(getUserEducationsQueryOptions(userSlug));

    return {
      user,
      experiences,
      educations,
      isLoading: userLoading || experiencesLoading || educationsLoading,
    };
};
`
### **1. Loading State Types**

```typescript
import { LoadingState } from '@/components/ui';

// Spinner loading
<LoadingState type="spinner" message="Loading..." />

// Dots loading
<LoadingState type="dots" />

// Pulse loading
<LoadingState type="pulse" message="Processing..." />


// Specialized loading states
<PageLoading />
<ContentLoading />
<CardLoading />
<ListLoading count={3} />


function MyComponent() {
  const [isLoading, setIsLoading] = useState(false);

  if (isLoading) {
    return <LoadingState type="spinner" message="Loading data..." />;
  }

  return <div>Content loaded!</div>;
}


import { MainErrorFallback, NetworkError, NotFound, Unauthorized } from '@/components/errors';

// Main error fallback
<MainErrorFallback
  error={error}
  customMessage="Custom error message"
  showHomeButton={true}
  showBackButton={true}
/>

// Network error
<NetworkError
  onRetry={() => refetch()}
  customMessage="Custom network error message"
/>

// Not found (404)
<NotFound
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
````

### **2. Specialized Loading Components**

```typescript
import { PageLoading, ContentLoading, CardLoading, ListLoading } from '@/components/ui';

// Basic skeleton
<Skeleton className="w-full h-32" />

// Full page loading
<PageLoading />

// Content area loading
<ContentLoading />

// Card loading
<CardLoading />

// List loading with custom count
<ListLoading count={5} />

// List skeleton with custom count
<ListSkeleton count={5} />

// Table skeleton
<TableSkeleton rows={10} columns={4} />
```

## 🔄 **Query Wrapper Examples**

### **2. Custom Loading Fallback**

```typescript
<QueryWrapper
  query={userQuery}
  loadingFallback={<CustomLoadingComponent />}
  errorFallback={(error) => <CustomErrorComponent error={error} />}
>
  {(user) => <UserProfile user={user} />}
</QueryWrapper>
```

### **3. Skeleton Types**

```typescript
// Profile skeleton
<QueryWrapper query={userQuery} skeletonType="profile">
  {(user) => <UserProfile user={user} />}
</QueryWrapper>

// List skeleton
<QueryWrapper query={usersQuery} skeletonType="list" skeletonProps={{ count: 5 }}>
  {(users) => <UserList users={users} />}
</QueryWrapper>

// Card skeleton
<QueryWrapper query={dataQuery} skeletonType="card">
  {(data) => <DataCard data={data} />}
</QueryWrapper>
```

### **4. Specialized Wrappers**

```typescript
import { ProfileQueryWrapper, ListQueryWrapper, CardQueryWrapper } from '@/components/wrappers/query-wrapper';

// Profile wrapper
<ProfileQueryWrapper query={userQuery}>
  {(user) => <UserProfile user={user} />}
</ProfileQueryWrapper>

// List wrapper
<ListQueryWrapper query={usersQuery} count={10}>
  {(users) => <UserList users={users} />}
</ListQueryWrapper>

// Card wrapper
<CardQueryWrapper query={dataQuery}>
  {(data) => <DataCard data={data} />}
</CardQueryWrapper>
```

## 🌍 **Real-World Scenarios**

### **1. E-commerce Product Listing**

```typescript
export const EcommerceProductListing = () => {
  const [category, setCategory] = useState('electronics');
  const [page, setPage] = useState(1);

  const productsQuery = useQuery({
    queryKey: ['products', category, page],
    queryFn: () => getProducts(category, page),
    keepPreviousData: true
  });

  return (
    <QueryWrapper
      query={productsQuery}
      skeletonType="list"
      skeletonProps={{ count: 8 }}
    >
      {(data) => (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {data.products.map(product => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      )}
    </QueryWrapper>
  );
};
```

### Custom Error Handling

```typescript
import { QueryWrapper } from '@/components/wrappers/query-wrapper';
import { NetworkError } from '@/components/errors';

function DataComponent() {
  const dataQuery = useQuery({
    queryKey: ['data'],
    queryFn: fetchData
  });

  return (
    <QueryWrapper
      query={dataQuery}
      errorFallback={(error) => {
        if (error.message.includes('Network')) {
          return <NetworkError onRetry={() => dataQuery.refetch()} />;
        }
        return <MainErrorFallback error={error} />;
      }}
    >
      {(data) => <DataDisplay data={data} />}
    </QueryWrapper>
  );
}
```

## ⚠️ **Error Handling Patterns**

### **4. Conditional Error Handling**

```typescript
export const ConditionalErrorHandling = () => {
  const dataQuery = useQuery({
    queryKey: ['conditional-data'],
    queryFn: getData,
    retry: false
  });

  const getErrorComponent = (error: Error) => {
    const message = error.message.toLowerCase();

    if (message.includes('network')) {
      return <NetworkError onRetry={() => dataQuery.refetch()} />;
    }

    if (message.includes('not found')) {
      return <NotFound title="Resource Not Found" message={error.message} />;
    }

    if (message.includes('unauthorized')) {
      return <Unauthorized title="Access Denied" message={error.message} />;
    }

    return <MainErrorFallback error={error} />;
  };

  return (
    <QueryWrapper
      query={dataQuery}
      errorFallback={getErrorComponent}
    >
      {(data) => <DataDisplay data={data} />}
    </QueryWrapper>
  );
};
```

### **3. Global Error Handling**

```typescript
export const GlobalErrorHandling = () => {
  const [globalError, setGlobalError] = useState<Error | null>(null);

  if (globalError) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background p-4">
        <div className="max-w-md w-full text-center space-y-6">
          <div className="mx-auto w-16 h-16 bg-red-100 rounded-full flex items-center justify-center">
            <span className="text-2xl">⚠️</span>
          </div>

          <div className="space-y-2">
            <h1 className="text-2xl font-bold text-foreground">
              Application Error
            </h1>
            <p className="text-muted-foreground">
              {globalError.message}
            </p>
          </div>

          <div className="flex gap-3 justify-center">
            <Button onClick={() => setGlobalError(null)}>
              Try Again
            </Button>
            <Button variant="outline" onClick={() => window.location.reload()}>
              Reload Page
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return <YourApp />;
};
```

## 📋 **Best Practices Summary**

### **Loading States**

- ✅ Use appropriate skeleton types for different content
- ✅ Provide meaningful loading messages
- ✅ Show progress for long operations
- ✅ Use optimistic updates where appropriate

### **Error Handling**

- ✅ Implement graceful degradation
- ✅ Use retry mechanisms with exponential backoff
- ✅ Provide clear error messages
- ✅ Offer recovery options
- ✅ Log errors for debugging

### **User Experience**

- ✅ Prevent layout shifts with skeletons
- ✅ Show loading states immediately
- ✅ Provide feedback for user actions
- ✅ Handle edge cases gracefully

### **Performance**

- ✅ Use React Query for caching
- ✅ Implement proper loading states
- ✅ Optimize bundle size
- ✅ Use error boundaries
