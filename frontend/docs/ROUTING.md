# Centralized Routing System

This document explains how to use the new centralized routing system in the booking monorepo frontend.

## Overview

The centralized routing system provides a single source of truth for all route definitions, making it easier to:

- Maintain and update routes
- Ensure consistency across the application
- Type-safe route generation
- Programmatic navigation
- Route-based utilities

## File Structure

```
src/
├── config/
│   ├── routes.ts           # Main route definitions and builders
│   ├── route-registry.ts   # Route mapping and utilities
│   └── paths.ts           # Backward-compatible path helpers
├── hooks/
│   └── use-navigation.ts  # Navigation hook and utilities
└── routes/
    └── centralized-routes.ts # Legacy file (deprecated)
```

## Usage Examples

### 1. Basic Route Generation

```typescript
import { routes } from '@/config/routes';

// Simple routes
const homeUrl = routes.to.home(); // "/"
const loginUrl = routes.to.auth.login(); // "/auth/login"
const dashboardUrl = routes.to.app.dashboard(); // "/app"

// Routes with parameters
const profileUrl = routes.to.profile.user('john'); // "/profile/john"
const bookingUrl = routes.to.booking.session('mentor-123'); // "/booking/session/mentor-123"

// Routes with query parameters
const loginWithRedirect = routes.to.auth.login({ redirectTo: '/app' });
// "/auth/login?redirectTo=%2Fapp"

const resetPassword = routes.to.auth.resetPassword({
  email: 'user@example.com',
  token: 'abc123',
  redirectTo: '/app',
});
// "/auth/reset-password?email=user%40example.com&token=abc123&redirectTo=%2Fapp"
```

### 2. Navigation Hook

```typescript
import { useAppNavigation } from '@/hooks/use-navigation';

function MyComponent() {
  const nav = useAppNavigation();

  const handleLogin = () => {
    nav.goToLogin({ redirectTo: '/app' });
  };

  const handleViewProfile = (userId: string) => {
    nav.goToProfile(userId);
  };

  const handleBookSession = (mentorSlug: string) => {
    nav.goToBookingSession(mentorSlug);
  };

  return (
    <div>
      <button onClick={handleLogin}>Login</button>
      <button onClick={() => handleViewProfile('john')}>View Profile</button>
      <button onClick={() => handleBookSession('mentor-123')}>Book Session</button>
    </div>
  );
}
```

### 3. Route Constants (Direct Access)

```typescript
import { ROUTE_PATHS } from '@/config/routes';

// Use constants directly
const authRoutes = [
  ROUTE_PATHS.AUTH.LOGIN, // "/auth/login"
  ROUTE_PATHS.AUTH.REGISTER, // "/auth/register"
  ROUTE_PATHS.AUTH.FORGOT_PASSWORD, // "/auth/forgot-password"
];

// Check if route is auth-related
const isAuthRoute = location.pathname.startsWith('/auth');
```

### 4. Route Utilities

```typescript
import { routeUtils } from '@/config/route-registry';

// Check route types
const isProtected = routeUtils.isProtectedRoute('/app/dashboard'); // true
const isPublic = routeUtils.isPublicRoute('/auth/login'); // true

// Get route group
const group = routeUtils.getRouteGroup('/booking/session/mentor-123'); // "BOOKING"

// Get file path for route
const filePath = routeUtils.getFilePath('/auth/login'); // "src/routes/auth/login.tsx"
```

### 5. Backward Compatibility (Legacy paths.ts)

```typescript
import { paths } from '@/config/paths';

// Still works with existing code
const loginUrl = paths.auth.login.getHref('/app'); // "/auth/login?redirectTo=%2Fapp"
const homeUrl = paths.home.getHref(); // "/"
const profileUrl = paths.profile.user.getHref('john'); // "/profile/john"
```

## Route Definitions

### Auth Routes

- `/auth/login` - Login page
- `/auth/register` - Registration page
- `/auth/forgot-password` - Forgot password page
- `/auth/reset-password` - Reset password page
- `/auth/email-verification/` - Email verification page
- `/auth/email-verification/verified` - Email verification success page

### App Routes

- `/app` - Main app dashboard

### Booking Routes

- `/booking/session/$mentorSlug` - Book a session with mentor
- `/booking/demo/$mentorSlug` - Demo booking page
- `/booking/enhanced/$mentorSlug` - Enhanced booking page
- `/booking/test/$mentorSlug` - Test booking page

### Mentor Routes

- `/mentor/become` - Become a mentor page
- `/mentor/set-schedule` - Set mentor schedule page

### Profile Routes

- `/profile/$userSlug` - User profile page

### Test/Error Routes

- `/test/*` - Various test pages
- `/error-exp/*` - Error experiment pages

## Migration Guide

### From hardcoded strings:

```typescript
// Old way ❌
navigate('/auth/login?redirectTo=' + encodeURIComponent('/app'));

// New way ✅
nav.goToLogin({ redirectTo: '/app' });
// or
navigate({ to: routes.to.auth.login({ redirectTo: '/app' }) });
```

### From legacy paths:

```typescript
// Old way (still works but deprecated)
const url = paths.auth.login.getHref('/app');

// New way ✅
const url = routes.to.auth.login({ redirectTo: '/app' });
```

### From route file definitions:

```typescript
// Old way ❌
export const Route = createFileRoute('/auth/login')({
  component: LoginPage,
});

// New way ✅ (in route files)
import { LoginPage } from '@/features/auth/pages/login-page';

export const Route = createFileRoute('/auth/login')({
  component: LoginPage,
  // ... other options
});
```

## Benefits

1. **Single Source of Truth**: All routes defined in one place
2. **Type Safety**: TypeScript ensures route parameters are correct
3. **Easy Refactoring**: Change a route in one place, updates everywhere
4. **Better Developer Experience**: Autocomplete for routes and parameters
5. **Consistency**: Standardized way to handle routes across the app
6. **Documentation**: Clear mapping of routes to files and features
7. **Utilities**: Built-in functions for route validation and navigation

## Best Practices

1. **Use the navigation hook** for programmatic navigation instead of direct navigate calls
2. **Use route builders** instead of string concatenation for URLs
3. **Keep route logic centralized** - don't hardcode routes in components
4. **Use route utilities** to check route types and extract parameters
5. **Update the route registry** when adding new routes
6. **Prefer typed navigation** over generic navigation where possible

## Adding New Routes

1. Add the route path to `ROUTE_PATHS` in `src/config/routes.ts`
2. Add a builder function to `routeBuilder` in the same file
3. Update the route registry in `src/config/route-registry.ts`
4. Add navigation methods to the navigation hook if needed
5. Update the paths.ts file for backward compatibility
6. Create the actual route file in the appropriate directory

This system ensures consistency and maintainability across the entire routing infrastructure.
