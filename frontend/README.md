# Booking Frontend

A modern React frontend application for a booking/marketplace platform built with Vite, TypeScript, and TanStack Router.

## Table of Contents

- [Overview](#overview)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [API Integration](#api-integration)
- [Getting Started](#getting-started)
- [Development Patterns](#development-patterns)
- [Error Handling Patterns](#error-handling-patterns)
- [Loading Handling Patterns](#loading-handling-patterns)
- [Search Params](#search-params)
- [Uncaught Errors and Not Found Pages](#uncaught-errors-and-not-found-pages)
- [TanStack Query Patterns](#tanstack-query-patterns)
- [Component Patterns](#component-patterns)
  - [Alert](#alert)
  - [Guards](#guards)
    - [AuthGuard](#authguard)
    - [StoreGuard](#storeguard)
  - [Navigation](#navigation)

## Overview

This frontend application provides a comprehensive user interface for the booking/marketplace platform with:

- **Type-safe routing** with TanStack Router for seamless navigation
- **Server state management** with TanStack Query for efficient data fetching
- **Real-time communication** with SignalR for live notifications
- **Modern UI components** built with Radix UI and Tailwind CSS
- **File upload capabilities** with drag-and-drop support
- **Form handling** with React Hook Form and Zod validation
- **Responsive design** optimized for desktop and mobile devices

## Technology Stack

- **React 18** - UI library with concurrent features
- **TypeScript** - Type safety and enhanced developer experience
- **TanStack Router** - Type-safe, file-based routing
- **TanStack Query** - Powerful data synchronization for React
- **Tailwind CSS v4** - Utility-first CSS framework
- **Radix UI (Shadcn)** - Unstyled, accessible UI primitives
- **React Hook Form** - Performant forms with easy validation
- **Zod** - TypeScript-first schema validation
- **SignalR** - Real-time web functionality
- **Axios** - Promise-based HTTP client
- **Sonner** - Toast notifications
- **class-variance-authority** - Component variant management

## Project Structure

```
frontend/
├── src/
│   ├── api/                              # API integration layer
│   │   ├── auth/                         # Authentication API endpoints
│   │   │   ├── login.ts                  # User login functionality
│   │   │   ├── logout.ts                 # User logout functionality
│   │   │   ├── register.ts               # User registration
│   │   │   ├── forgot-password.ts        # Password recovery
│   │   │   ├── reset-password.ts         # Password reset
│   │   │   ├── verify-email.ts           # Email verification
│   │   │   ├── oauth-api.ts              # OAuth integration (Google, etc.)
│   │   │   ├── user.ts                   # User profile management
│   │   │   ├── use-auth.ts               # Authentication hooks
│   │   │   └── auth-keys.ts              # Query keys for auth
│   │   ├── stores/                       # Store management endpoints
│   │   ├── utils/                        # API utility functions
│   │   │   ├── api-client.ts             # Core HTTP client with auth
│   │   │   ├── fetcher.ts                # TanStack Query fetcher
│   │   │   ├── auth-endpoints.ts         # Auth endpoint definitions
│   │   │   ├── catalog-endpoints.ts      # Catalog endpoint definitions
│   │   │   ├── to-form-data.ts           # FormData conversion utility
│   │   │   └── validate-file.ts          # File validation helpers
│   │   └── index.ts                      # API exports
│   ├── assets/                           # Static assets
│   │   ├── calendar-image.jpeg           # UI images
│   │   ├── logo.svg                      # Application logos
│   │   ├── logo_m.png                    # Mobile logo variant
│   │   ├── fallback-session-product-thumbnail.jpg # Fallback images
│   │   └── index.ts                      # Asset exports
│   ├── components/                       # Reusable UI components
│   │   ├── errors/                       # Error handling components
│   │   ├── guards/                       # Route protection components
│   │   ├── headers/                      # Header and navigation
│   │   ├── layouts/                      # Layout components
│   │   ├── navigation/                   # Navigation components
│   │   ├── pages/                        # Page-specific components
│   │   ├── seo/                          # SEO optimization components
│   │   ├── shared/                       # Shared utility components
│   │   ├── store/                        # Store-related components
│   │   ├── ui/                           # Base UI components (Radix UI)
│   │   ├── wrappers/                     # HOC and wrapper components
│   │   ├── logo.tsx                      # Logo component
│   │   ├── scrollable-content.tsx        # Scrollable container
│   │   ├── tab-navigation.tsx            # Tab navigation
│   │   ├── theme-toggle.tsx              # Dark/light mode toggle
│   │   ├── upload-image.tsx              # File upload component
│   │   └── index.ts                      # Component exports
│   ├── config/                           # Application configuration
│   │   ├── env.ts                        # Environment variable validation
│   │   ├── routes.ts                     # Route constants
│   │   └── index.ts                      # Config exports
│   ├── features/                         # Feature-based state management
│   │   ├── admin/                        # Admin panel features
│   │   ├── app/                          # Core app features
│   │   ├── auth/                         # Authentication state
│   │   ├── public/                       # Public features
│   │   └── index.ts                      # Feature exports
│   ├── hooks/                            # Custom React hooks
│   │   ├── use-debounce.ts               # Input debouncing
│   │   ├── use-media-query.ts            # Responsive breakpoints
│   │   ├── use-navigation.ts             # Navigation utilities
│   │   ├── use-outside-click.ts          # Click outside detection
│   │   ├── use-query-state.ts            # URL state management
│   │   ├── use-time-filter.ts            # Time filtering logic
│   │   ├── use-upload-picture.tsx        # File upload handling
│   │   └── index.ts                      # Hook exports
│   ├── lib/                              # Core utilities and helpers
│   │   ├── cn.ts                         # Tailwind class name utility
│   │   ├── consts.ts                     # Application constants
│   │   ├── deep-copy.ts                  # Object cloning utility
│   │   ├── id.ts                         # ID generation
│   │   ├── utils.ts                      # General utilities
│   │   └── index.ts                      # Library exports
│   ├── providers/                        # React context providers
│   │   ├── app-provider.tsx              # Main app provider
│   │   ├── react-query.tsx               # TanStack Query setup
│   │   └── react-router.tsx              # Router configuration
│   ├── routes/                           # File-based routing (TanStack Router)
│   │   ├── __root.tsx                    # Root layout component
│   │   ├── index.tsx                     # Landing page
│   │   ├── home.tsx                      # Home page
│   │   ├── unauthorized.tsx              # 403 error page
│   │   ├── (public)/                     # Public routes group
│   │   ├── app/                          # Protected app routes
│   │   ├── auth/                         # Authentication routes
│   │   ├── error-exp/                    # Error experiment routes
│   │   ├── test/                         # Testing routes
│   │   └── routeTree.gen.ts              # Auto-generated route tree
│   ├── services/                         # Business logic services
│   │   ├── date/                         # Date/time utilities
│   │   ├── image/                        # Image processing
│   │   ├── indexedDB/                    # Client-side storage
│   │   ├── notifications/                # Push notifications
│   │   └── pixels/                       # Analytics tracking
│   ├── stores/                           # Global state management
│   │   └── store.ts                      # Redux store configuration
│   ├── types/                            # TypeScript type definitions
│   │   ├── Budget.ts                     # Budget-related types
│   │   ├── Category.ts                   # Category data types
│   │   ├── Image.ts                      # Image handling types
│   │   ├── Order.ts                      # Order management types
│   │   ├── Payment.ts                    # Payment types
│   │   ├── Product.ts                    # Product catalog types
│   │   ├── Staff.ts                      # Staff management types
│   │   ├── Statistics.ts                 # Analytics types
│   │   ├── Store.ts                      # Store types
│   │   └── Upsell.ts                     # Upselling types
│   ├── utils/                            # Utility functions
│   │   ├── balance-utils.ts              # Currency formatting
│   │   ├── invoice-utils.ts              # Invoice generation
│   │   └── redirect-utils.ts             # Navigation helpers
│   ├── main.tsx                          # Application entry point
│   ├── index.css                         # Global styles
│   └── vite-env.d.ts                     # Vite type definitions
├── .tanstack/                            # TanStack Router cache
├── docs/                                 # Project documentation
│   ├── form.md                           # Form patterns
│   ├── ROUTING.md                        # Routing guide
│   ├── SCRIPTS_AND_ESLINT.md            # Development workflow
│   └── zod.md                           # Schema validation
├── eslint-rules/                         # Custom ESLint rules
│   ├── enforce-route-constants.js        # Route validation
│   └── route-config.js                   # Route configuration
├── public/                               # Static assets
│   ├── google-calendar.png               # Integration assets
│   └── konnect.svg                       # Service logos
├── scripts/                              # Build and development scripts
│   ├── check-imports.js                  # Import validation
│   └── find-navigation-patterns.js       # Pattern analysis
├── components.json                       # Radix UI configuration
├── eslint.config.js                      # ESLint rules
├── package.json                          # Dependencies and scripts
├── tailwind.config.cjs                   # Tailwind configuration
├── tsconfig.json                         # TypeScript config
├── vite.config.ts                        # Vite build config
└── README.md                            # Project documentation
```

## API Integration

The frontend connects to the .NET backend through a comprehensive API integration layer that handles authentication, data fetching, and real-time communication.

### Core API Architecture

#### 1. **HTTP Client Setup** (`src/api/utils/api-client.ts`)

The application uses a custom API client built on the Fetch API with:

- **Automatic authentication** - JWT tokens sent via HTTP-only cookies
- **Request/response interceptors** - For error handling and token refresh
- **Type-safe requests** - Full TypeScript support for API calls
- **Automatic retry logic** - For failed requests with exponential backoff

```typescript
// Example API client usage
const response = await fetchApi<User>('users/profile', {
  method: 'GET',
  params: { includePreferences: true },
});
```

#### 2. **Environment Configuration** (`src/config/env.ts`)

Environment variables are validated using Zod schemas:

- `VITE_API_URL` - Backend API base URL (default: http://localhost:5000)
- `VITE_APP_URL` - Frontend app URL (default: http://localhost:3000)
- `VITE_ENVIRONMENT` - Current environment (development/staging/production)

#### 3. **Authentication Flow** (`src/api/auth/`)

**Available Auth Endpoints:**

- `login.ts` - Email/password and OAuth login
- `register.ts` - New user registration with email verification
- `oauth-api.ts` - Google OAuth integration
- `forgot-password.ts` / `reset-password.ts` - Password recovery flow
- `verify-email.ts` - Email verification process

**Auth Hook Usage:**

#### useAuth Hook (`src/api/auth/use-auth.ts`)

A comprehensive authentication hook that combines all auth-related functionality into a single interface:

```typescript
import { useUser, useLogin, useRegister, useLogout, useForgotPassword, useResetPassword } from '@/api/auth';
import { useAppNavigation } from '@/hooks';

export const useAuth = () => {
  const navigate = useAppNavigation();
  const { data: currentUser, isLoading, error } = useUser();
  const login = useLogin();
  const register = useRegister();

  const logout = useLogout({
    onSuccess: () => {
      navigate.goToLogin();
    },
  });
  const forgotPassword = useForgotPassword();
  const resetPassword = useResetPassword();

  return {
    // State
    currentUser,
    isLoading,
    error,
    isAuthenticated: !!currentUser,

    // Actions
    login: login.mutate,
    register: register.mutate,
    logout: logout.mutate,
    forgotPassword: forgotPassword.mutate,
    resetPassword: resetPassword.mutate,

    // Loading states
    isLoggingIn: login.isPending,
    isRegistering: register.isPending,
    isLoggingOut: logout.isPending,
    isForgettingPassword: forgotPassword.isPending,
    isResettingPassword: resetPassword.isPending,
  };
};
```

**Hook Features:**

- **State Management**: Current user data, loading states, and authentication status
- **Actions**: Login, register, logout, password reset operations
- **Loading States**: Individual loading states for each auth operation
- **Navigation**: Automatic redirect to login page on logout
- **Error Handling**: Comprehensive error states for all operations

**Usage Example:**

```typescript
import { useAuth } from '@/api/auth';

function LoginForm() {
  const { login, isLoggingIn, error } = useAuth();

  const handleSubmit = (credentials) => {
    login(credentials, {
      onSuccess: () => {
        // Redirect to dashboard
      },
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Form fields */}
      {error && <ErrorMessage error={error} />}
      <button disabled={isLoggingIn}>
        {isLoggingIn ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
}
```

### TanStack Query Integration

#### 4. **Server State Management**

The app uses TanStack Query for efficient server state management:

**Query Keys** (`src/api/auth/auth-keys.ts`):

```typescript
export const authKeys = {
  all: ['auth'] as const,
  user: () => [...authKeys.all, 'user'] as const,
  profile: (userId: string) => [...authKeys.user(), userId] as const,
};
```

**Custom Fetcher** (`src/api/utils/fetcher.ts`):

- Performance monitoring for API calls
- Consistent error handling across queries
- Request deduplication and caching

**Query Provider Setup** (`src/providers/react-query.tsx`):

```typescript
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000, // 5 minutes
      cacheTime: 10 * 60 * 1000, // 10 minutes
      retry: (failureCount, error) => {
        // Custom retry logic based on error type
        return failureCount < 3 && error.status !== 404;
      },
    },
  },
});
```

### Real-time Communication

#### 5. **SignalR Integration**

Real-time features powered by SignalR:

- **Live notifications** - Order updates, payment confirmations
- **Real-time collaboration** - Multi-user editing capabilities
- **Connection management** - Automatic reconnection on network issues

### API Endpoint Organization

#### 6. **Modular Endpoint Structure**

**Authentication Endpoints** (`src/api/utils/auth-endpoints.ts`):

- User login/logout/registration
- Password reset and email verification
- OAuth provider integration
- Token refresh and validation

**Catalog Endpoints** (`src/api/utils/catalog-endpoints.ts`):

- Product and service management
- Store operations and analytics
- Order processing and tracking
- Payment and payout handling

### File Upload Handling

#### 7. **Multi-format Upload Support**

**Upload Component** (`src/components/upload-image.tsx`):

- Drag-and-drop interface
- Multiple file selection
- Progress tracking and preview
- Image optimization and validation

**Upload Utilities** (`src/api/utils/`):

- `validate-file.ts` - File type and size validation
- `to-form-data.ts` - FormData conversion for multipart uploads

### Error Handling & Loading States

#### 8. **Comprehensive Error Management**

**Error Boundaries** (`src/components/errors/`):

- Network error recovery
- 404 and 403 error pages
- Fallback UI components

**Loading States** (`src/features/`):

- Global loading indicators
- Skeleton loaders for content
- Optimistic updates for better UX

### Development Tools

#### 9. **API Development Workflow**

**Development Scripts**:

- `npm run dev` - Start development server with API proxy
- `npm run type-check` - Validate TypeScript across API types
- `npm run check:imports` - Validate import paths and dependencies

This architecture ensures type safety, performance, and maintainability while providing a seamless developer experience for API integration.

### Local Development Setup

1. **Install dependencies**

   ```bash
   npm install
   ```

2. **Configure environment**

   ```bash
   cp .env.example .env.local
   # Edit .env.local with your configuration
   ```

3. **Start development server**

   ```bash
   npm run dev
   ```

4. **Build for production**
   ```bash
   npm run build
   ```

The application will be available at `http://localhost:5173`

## Development Patterns

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

```

### Error Component Selection Logic

```ts
const getErrorComponent = (error: any) => {
  const status = error.status || error.response?.status;

  if (!status || status === 0) {
    return <NetworkError onRetry={() => dataQuery.refetch()}} />;

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

## Search Params (Query : ?key=Value)

### Basic Usage

```ts
// To use search params, configure the route file:
const { redirectTo, token, email } = useSearch({ strict: false });

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

### Acess Params

### Accessing Search Params

```ts
export const StorePreview = () => {
  const params = useParams({ strict: false }) as Record<string, string | undefined>;
  return <div>Store Preview Component - Store Slug: {params.storeSlug}</div>;
};
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
