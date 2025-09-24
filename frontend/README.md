# Search params

```ts
  const routeApi = getRouteApi(ROUTE_PATHS.AUTH.LOGIN);
  const { redirectTo } = routeApi.useSearch();

  ---
    const search = useSearch({ strict: false });
  const redirectTo = (search.redirectTo as string) || './app';
```

# Patterns

## Alert

The Alert component is used to display notifications, such as success or error messages after form submissions.

### Icon Mapping

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

### Error Alert Example

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

### Success Alert Example

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

## Guards

## AuthGuard

The `AuthGuard` component is a higher-order component that handles authentication and authorization for protected routes.

### Behavior

- Shows a loading spinner while checking authentication status.
- Redirects authenticated users away from auth pages.
- Redirects unauthenticated users to login with return path.
- Renders children only when access conditions are met.

### Usage Example

```tsx
import { AuthGuard } from '@/components/guards/auth-guard';

<AuthGuard requireAuth>
  <ProtectedComponent />
</AuthGuard>;
```

---

## StoreGuard

The `StoreGuard` component is a higher-order component that enforces users to set up their store before proceeding.

### Behavior

- Shows a loading spinner while checking if the store is set up.
- Redirects users without a store to the setup page.
- Renders children only when the store is set up.

### Usage Example

```tsx
import { StoreGuard } from '@/components/guards/store-guard';

<StoreGuard>
  <ComponentRequiringStore />
</StoreGuard>;
```
