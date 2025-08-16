
## Usage Examples

### Basic Navigation

```typescript
import { useAppNavigation } from '@/hooks/use-navigation';

function MyComponent() {
  const { navigateToAuth, navigateToBooking } = useAppNavigation();

  const handleLogin = () => navigateToAuth.login();
  const handleBookSession = (mentorSlug: string) =>
    navigateToBooking.session(mentorSlug);
}
```

### Route Building

```typescript
import { routes } from '@/config/routes';

// Simple routes
const loginUrl = routes.to.auth.login(); // '/auth/login'

// Parameterized routes
const userProfile = routes.to.profile.user('john-doe'); // '/profile/john-doe'
const bookingSession = routes.to.booking.session('jane-smith'); // '/booking/session/jane-smith'

// Route checking
const isAuthRoute = routes.matchesRoute('/auth/login', ROUTE_PATHS.AUTH.LOGIN); // true
```

### Adding New Routes

1. **Add to constants** in `src/config/routes.ts`:

```typescript
export const ROUTE_PATHS = {
  // ... existing routes
  ADMIN: {
    DASHBOARD: '/admin/dashboard',
    USERS: '/admin/users',
  },
};
```

2. **Create route file** using the constant:

```typescript
// src/routes/admin/dashboard.tsx
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.ADMIN.DASHBOARD)({
  component: AdminDashboard,
});
```

3. **Validate synchronization**:

```bash
npm run check:routes
```
