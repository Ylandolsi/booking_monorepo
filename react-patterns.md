# Advanced React Patterns Guide

A comprehensive guide to modern React patterns with real-world examples and use cases.

---

## Table of Contents

1. [Utility Patterns](#utility-patterns)
   - [Performance Monitoring Pattern](#performance-monitoring-pattern)
   - [Single Flight Pattern](#single-flight-pattern)
   - [Config-Driven Conditional Rendering](#config-driven-conditional-rendering)
2. [Component Patterns](#component-patterns)
   - [Compound Components Pattern](#compound-components-pattern)
   - [Container-Presentational Pattern](#container-presentational-pattern)
   - [Render Props Pattern](#render-props-pattern)
   - [Variant Props Pattern](#variant-props-pattern)

---

## Utility Patterns

### Performance Monitoring Pattern

#### Description

Wraps API calls with timing measurement to track performance and identify bottlenecks in your application.

#### When to Use

- **Debugging slow API calls** in development
- **Monitoring production performance** to identify which endpoints need optimization
- **Creating performance dashboards** with real user metrics
- **Identifying network bottlenecks** in complex data flows

#### Real-World Scenarios

- E-commerce checkout: Track which API calls slow down the purchase flow
- Dashboard loading: Identify which data fetches are causing slow initial renders
- Search functionality: Measure autocomplete API response times
- File uploads: Monitor upload speed and identify issues

#### Implementation

```typescript
// utils/fetcher.ts
export default function fetcher(f: () => Promise<any>, key: unknown) {
  return async () => {
    const start = performance.now();
    const result = await f();
    const end = performance.now();
    console.log(`${JSON.stringify(key)}: ${end - start}ms`);
    return result;
  };
}
```

#### Usage Example

```typescript
import useSWR from "swr";
import fetcher from "./utils/fetcher";

function UserProfile({ userId }: { userId: string }) {
  const { data, error } = useSWR(
    `/api/users/${userId}`,
    fetcher(
      () => fetch(`/api/users/${userId}`).then((res) => res.json()),
      `user-${userId}`
    )
  );

  if (error) return <div>Failed to load</div>;
  if (!data) return <div>Loading...</div>;

  return <div>{data.name}</div>;
}
```

#### Enhanced Version with Analytics

```typescript
export function fetcherWithAnalytics(
  f: () => Promise<any>,
  key: unknown,
  options?: { threshold?: number }
) {
  return async () => {
    const start = performance.now();
    const result = await f();
    const end = performance.now();
    const duration = end - start;

    console.log(`${JSON.stringify(key)}: ${duration}ms`);

    // Send to analytics if slow
    if (options?.threshold && duration > options.threshold) {
      analytics.track("slow_api_call", {
        endpoint: key,
        duration,
        timestamp: new Date().toISOString(),
      });
    }

    return result;
  };
}
```

---

### Single Flight Pattern

#### Description

Prevents multiple simultaneous requests to the same endpoint by deduplicating in-flight API calls. If a request is already pending, return the existing promise instead of making a new request.

#### When to Use

- **Preventing race conditions** when the same data is requested multiple times
- **Optimizing rapid user interactions** (e.g., debounced search, double-clicks)
- **Reducing server load** from duplicate requests
- **Managing concurrent component renders** requesting the same data

#### Real-World Scenarios

- **Search autocomplete**: User types quickly, but only one request for "react" should be made
- **Tab switching**: Multiple tabs request user data, but only fetch once
- **Refresh buttons**: Prevent double-clicks from triggering duplicate fetches
- **Concurrent component mounts**: Multiple components need the same data simultaneously

#### Implementation

```typescript
// utils/singleFlight.ts
type AsyncFunction<V> = () => Promise<V>;

const pendingPromises: Map<string, Promise<any>> = new Map();

const run = async <V>(key: string, fn: AsyncFunction<V>): Promise<V> => {
  // Check if request is already in flight
  if (pendingPromises.has(key)) {
    return pendingPromises.get(key) as Promise<V>;
  }

  // Start new request
  const promise = fn();
  pendingPromises.set(key, promise);

  try {
    const result = await promise;
    return result;
  } finally {
    // Clean up after completion
    pendingPromises.delete(key);
  }
};

export default { run };
```

#### Usage Examples

##### Example 1: Search Autocomplete

```typescript
import singleFlight from "./utils/singleFlight";

async function searchProducts(query: string) {
  return singleFlight.run(`search-${query}`, () =>
    fetch(`/api/search?q=${query}`).then((res) => res.json())
  );
}

function SearchBar() {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState([]);

  const handleSearch = async (value: string) => {
    setQuery(value);

    // Even if called multiple times rapidly, only one request per query
    const data = await searchProducts(value);
    setResults(data);
  };

  return (
    <input
      value={query}
      onChange={(e) => handleSearch(e.target.value)}
      placeholder="Search products..."
    />
  );
}
```

##### Example 2: Preventing Duplicate Submissions

```typescript
async function submitOrder(orderId: string, data: OrderData) {
  return singleFlight.run(`submit-order-${orderId}`, () =>
    fetch(`/api/orders/${orderId}`, {
      method: "POST",
      body: JSON.stringify(data),
    }).then((res) => res.json())
  );
}

function CheckoutButton({ orderId, orderData }: Props) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async () => {
    setIsSubmitting(true);
    try {
      // Prevents double-click submissions
      await submitOrder(orderId, orderData);
      showSuccess("Order placed!");
    } catch (error) {
      showError("Failed to place order");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <button onClick={handleSubmit} disabled={isSubmitting}>
      Place Order
    </button>
  );
}
```

##### Example 3: Shared Data Across Components

```typescript
// Multiple components can call this simultaneously
async function fetchUserProfile(userId: string) {
  return singleFlight.run(`user-profile-${userId}`, () =>
    fetch(`/api/users/${userId}`).then((res) => res.json())
  );
}

function UserAvatar({ userId }: { userId: string }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    fetchUserProfile(userId).then(setUser);
  }, [userId]);

  return <img src={user?.avatar} />;
}

function UserName({ userId }: { userId: string }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    fetchUserProfile(userId).then(setUser);
  }, [userId]);

  return <span>{user?.name}</span>;
}

// Both components mount at the same time = only 1 API call
function UserCard({ userId }: { userId: string }) {
  return (
    <div>
      <UserAvatar userId={userId} />
      <UserName userId={userId} />
    </div>
  );
}
```

---

### Config-Driven Conditional Rendering

#### Description

Uses an array of configuration objects to determine which component or message to render based on conditions. The first config object where all conditions return true is selected.

#### When to Use

- **Complex conditional rendering** with multiple scenarios
- **Feature flags and A/B testing** configurations
- **Dynamic banners and notifications** based on user state
- **Multi-step form validation** with different error messages
- **Permission-based UI rendering**

#### Real-World Scenarios

- **Billing reminders**: Show different messages based on subscription status
- **Onboarding flows**: Display different tooltips based on user progress
- **Error handling**: Show specific error messages based on error type
- **Promotional banners**: Display different offers based on user segment

#### Implementation

```typescript
interface ReminderConfig {
  name: string;
  condition: (conditions: any) => boolean;
  locationCheck: (pathname: string, searchParams: URLSearchParams) => boolean;
  message: (data?: any) => JSX.Element;
  image: string;
}

const reminderConfigs: ReminderConfig[] = [
  {
    name: "exceededRevenueLimit",
    condition: ({ exceededRevenueLimit }) => exceededRevenueLimit,
    locationCheck: (pathname) => pathname === "/",
    message: () => (
      <p>
        You've exceeded your current plan's revenue limit. <br />
        Upgrade now and pay the difference.
      </p>
    ),
    image: ArrowImage,
  },
  {
    name: "balanceAlmostConsumed",
    condition: ({ balanceAlmostConsumed }) => balanceAlmostConsumed,
    locationCheck: (pathname) => pathname === "/",
    message: () => (
      <p>
        Your balance is almost consumed. <br />
        Recharge now to keep receiving new orders.
      </p>
    ),
    image: TimerImage,
  },
  {
    name: "reached90PercentOfRevenueLimit",
    condition: ({ reached90PercentOfRevenueLimit, currentSubscription }) =>
      reached90PercentOfRevenueLimit && currentSubscription != null,
    locationCheck: (pathname, searchParams) =>
      pathname === "/billing" && searchParams.get("tab") === "Subscription",
    message: ({ currentSubscription }) => (
      <p>
        Your current plan covers up to{" "}
        <span className="font-bold">
          ${currentSubscription?.plan.monthlyRevenueLimit}
        </span>{" "}
        in revenue.
        <br />
        You've reached 90% of your limit.
        <br />
        To continue receiving orders, please upgrade your plan.
      </p>
    ),
    image: TimerImage,
  },
];

// Usage
function ReminderBanner() {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const conditions = useSubscriptionStatus(); // Custom hook

  const activeReminder = reminderConfigs.find(
    (config) =>
      config.condition(conditions) &&
      config.locationCheck(location.pathname, searchParams)
  );

  if (!activeReminder) return null;

  return (
    <div className="reminder-banner">
      <img src={activeReminder.image} alt="" />
      {activeReminder.message(conditions)}
    </div>
  );
}
```

#### Another Example: Feature Access Control

```typescript
interface FeatureConfig {
  name: string;
  isEnabled: (user: User) => boolean;
  routeCheck: (pathname: string) => boolean;
  component: () => JSX.Element;
  fallback: () => JSX.Element;
}

const featureConfigs: FeatureConfig[] = [
  {
    name: "advancedAnalytics",
    isEnabled: (user) => user.plan === "premium" || user.plan === "enterprise",
    routeCheck: (pathname) => pathname.startsWith("/analytics"),
    component: () => <AdvancedAnalyticsDashboard />,
    fallback: () => <UpgradePrompt feature="Advanced Analytics" />,
  },
  {
    name: "teamCollaboration",
    isEnabled: (user) => user.plan === "enterprise" && user.role === "admin",
    routeCheck: (pathname) => pathname.startsWith("/team"),
    component: () => <TeamManagement />,
    fallback: () => <AccessDenied />,
  },
];

function FeatureGate() {
  const location = useLocation();
  const user = useCurrentUser();

  const activeFeature = featureConfigs.find(
    (config) => config.routeCheck(location.pathname) && config.isEnabled(user)
  );

  if (!activeFeature) {
    const blockedFeature = featureConfigs.find((config) =>
      config.routeCheck(location.pathname)
    );
    return blockedFeature?.fallback() || <NotFound />;
  }

  return activeFeature.component();
}
```

---

## Component Patterns

### Compound Components Pattern

#### Description

Multiple components work together as a cohesive unit. A parent component acts as a controller, while child components interact with shared state through Context.

#### When to Use

- **Building flexible, composable UI components** (dropdowns, tabs, accordions)
- **Creating component libraries** where users need customization options
- **Avoiding prop drilling** in complex component hierarchies
- **Providing better component APIs** with clear relationships

#### Real-World Scenarios

- **Custom select dropdowns** with searchable options
- **Tab interfaces** with dynamic content
- **Accordion menus** with multiple panels
- **Card components** with customizable sections

#### Implementation

```typescript
import { createContext, useContext, ReactNode } from "react";

type CardData = {
  title: string;
  content: string;
  action: string;
  footer: string;
};

type CardContextType = {
  data: CardData;
};

const CardContext = createContext<CardContextType | undefined>(undefined);

function useCardContext() {
  const context = useContext(CardContext);
  if (!context) {
    throw new Error("Card components must be used within a Card");
  }
  return context;
}

export default function Card({ children }: { children: ReactNode }) {
  const data: CardData = {
    title: "Card Title",
    content: "Card content",
    action: "Action",
    footer: "Card footer",
  };

  return (
    <CardContext.Provider value={{ data }}>
      <div className="card">{children}</div>
    </CardContext.Provider>
  );
}

Card.Title = function CardTitle() {
  const { data } = useCardContext();
  return (
    <div className="card-header">
      <h3>{data.title}</h3>
    </div>
  );
};

Card.Content = function CardContent() {
  const { data } = useCardContext();
  return (
    <div className="card-body">
      <p>{data.content}</p>
    </div>
  );
};

Card.Footer = function CardFooter() {
  const { data } = useCardContext();
  return (
    <div className="card-footer">
      <p>{data.footer}</p>
    </div>
  );
};

Card.Action = function CardAction() {
  const { data } = useCardContext();
  return (
    <div className="card-action">
      <button>{data.action}</button>
    </div>
  );
};

// Usage - Flexible composition
function ProductCard() {
  return (
    <Card>
      <Card.Title />
      <Card.Content />
      <Card.Action />
      <Card.Footer />
    </Card>
  );
}

// Can reorder or omit components
function SimpleCard() {
  return (
    <Card>
      <Card.Title />
      <Card.Content />
    </Card>
  );
}
```

---

### Container-Presentational Pattern

#### Description

Separates business logic (container) from UI rendering (presentational). Container components handle state and data fetching, while presentational components focus solely on displaying the UI.

#### When to Use

- **Improving component testability** by isolating logic from UI
- **Enabling component reuse** across different data sources
- **Simplifying complex components** with multiple responsibilities
- **Team collaboration** where different developers handle logic vs. styling

#### Real-World Scenarios

- **Data tables** where the table component is reused with different data sources
- **Form components** that can be used in different contexts
- **List views** that display different types of items
- **Dashboard widgets** that can show various metrics

#### Implementation

```typescript
// Presentational Component - Pure UI
interface UserListProps {
  users: User[];
  isLoading: boolean;
  onUserClick: (userId: string) => void;
}

function UserList({ users, isLoading, onUserClick }: UserListProps) {
  if (isLoading) {
    return <Spinner />;
  }

  return (
    <ul className="user-list">
      {users.map((user) => (
        <li key={user.id} onClick={() => onUserClick(user.id)}>
          <Avatar src={user.avatar} />
          <span>{user.name}</span>
        </li>
      ))}
    </ul>
  );
}

// Container Component - Logic and State
function UserListContainer() {
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("/api/users")
      .then((res) => res.json())
      .then((data) => {
        setUsers(data);
        setIsLoading(false);
      });
  }, []);

  const handleUserClick = (userId: string) => {
    navigate(`/users/${userId}`);
  };

  return (
    <UserList
      users={users}
      isLoading={isLoading}
      onUserClick={handleUserClick}
    />
  );
}
```

---

### Render Props Pattern

#### Description

A component shares logic by providing a function prop that controls what to render. The parent decides how the UI should look while the child manages the logic.

#### When to Use

- **Sharing stateful logic** without using hooks or HOCs
- **Providing granular control** over rendering
- **Building reusable data fetching components**
- **Creating flexible animation wrappers**

#### Real-World Scenarios

- **Data fetchers** that work with any UI
- **Mouse position trackers** for tooltips or custom cursors
- **Form validation** with custom error display
- **Scroll position handlers** for various effects

#### Implementation

```typescript
interface DataFetcherProps<T> {
  url: string;
  render: (data: T[], isLoading: boolean) => JSX.Element;
}

function DataFetcher<T>({ url, render }: DataFetcherProps<T>) {
  const [data, setData] = useState<T[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    fetch(url)
      .then((response) => response.json())
      .then((json) => {
        setData(json);
        setIsLoading(false);
      });
  }, [url]);

  return render(data, isLoading);
}

// Usage - Different UIs with same logic
type Post = {
  id: number;
  title: string;
  body: string;
};

function PostsList() {
  return (
    <DataFetcher<Post>
      url="https://jsonplaceholder.typicode.com/posts"
      render={(data, isLoading) => {
        if (isLoading) return <h1>Loading...</h1>;

        return (
          <div>
            <h1>Posts List</h1>
            {data.map((post) => (
              <article key={post.id}>
                <h2>{post.title}</h2>
                <p>{post.body}</p>
              </article>
            ))}
          </div>
        );
      }}
    />
  );
}

function PostsGrid() {
  return (
    <DataFetcher<Post>
      url="https://jsonplaceholder.typicode.com/posts"
      render={(data, isLoading) => {
        if (isLoading) return <Spinner />;

        return (
          <div className="grid">
            {data.map((post) => (
              <Card key={post.id}>
                <h3>{post.title}</h3>
                <p>{post.body}</p>
              </Card>
            ))}
          </div>
        );
      }}
    />
  );
}
```

---

### Variant Props Pattern

#### Description

Encodes variations in component behavior or styling through a single `variant` prop, reducing branching logic and improving code readability.

#### When to Use

- **Building design systems** with consistent styling variations
- **Creating flexible button/badge components** with multiple styles
- **Simplifying theme-based components**
- **Reducing prop explosion** in components with many style options

#### Real-World Scenarios

- **Button components** with primary, secondary, danger variants
- **Alert/notification components** with success, warning, error types
- **Badge components** for status indicators
- **Card components** with different elevations or borders

#### Implementation

```typescript
interface ButtonProps {
  variant?: "primary" | "secondary" | "danger" | "success" | "warning";
  title: string;
  onClick?: () => void;
}

function Button({ variant = "secondary", title, onClick }: ButtonProps) {
  const styles = variantStyles[variant];

  return (
    <button style={styles} onClick={onClick} className={`btn btn-${variant}`}>
      {title}
    </button>
  );
}

const variantStyles = {
  primary: {
    backgroundColor: "#007bff",
    color: "white",
    border: "none",
  },
  secondary: {
    backgroundColor: "#fff",
    color: "#000",
    border: "1px solid #ccc",
  },
  danger: {
    backgroundColor: "#dc3545",
    color: "#fff",
    border: "none",
  },
  warning: {
    backgroundColor: "#ffc107",
    color: "#000",
    border: "none",
  },
  success: {
    backgroundColor: "#28a745",
    color: "#fff",
    border: "none",
  },
};

// Usage
function ActionButtons() {
  return (
    <div>
      <Button variant="primary" title="Save" />
      <Button variant="secondary" title="Cancel" />
      <Button variant="danger" title="Delete" />
    </div>
  );
}
```

#### Advanced Example with Size Variants

```typescript
interface BadgeProps {
  variant?: "success" | "error" | "warning" | "info";
  size?: "sm" | "md" | "lg";
  children: ReactNode;
}

function Badge({ variant = "info", size = "md", children }: BadgeProps) {
  const variantClass = `badge-${variant}`;
  const sizeClass = `badge-${size}`;

  return (
    <span className={`badge ${variantClass} ${sizeClass}`}>{children}</span>
  );
}

// Usage
function OrderStatus({ status }: { status: string }) {
  const getVariant = () => {
    switch (status) {
      case "completed":
        return "success";
      case "pending":
        return "warning";
      case "failed":
        return "error";
      default:
        return "info";
    }
  };

  return <Badge variant={getVariant()}>{status}</Badge>;
}
```

---

## Combining Patterns

These patterns work great together. Here's an example combining multiple patterns:

```typescript
// Compound + Variant + Container-Presentational
function NotificationSystem() {
  // Container logic
  const { notifications } = useNotifications();

  return (
    <NotificationList>
      {notifications.map((notification) => (
        <Notification key={notification.id}>
          <Notification.Icon variant={notification.type} />
          <Notification.Content>
            <Notification.Title>{notification.title}</Notification.Title>
            <Notification.Message>{notification.message}</Notification.Message>
          </Notification.Content>
          <Notification.Action
            variant="primary"
            onClick={notification.onAction}
          >
            {notification.actionLabel}
          </Notification.Action>
        </Notification>
      ))}
    </NotificationList>
  );
}
```

---

## Best Practices

1. **Choose the right pattern** for your use case - don't over-engineer simple components
2. **Combine patterns** when they complement each other
3. **Document your patterns** so team members understand the approach
4. **Test patterns thoroughly** - especially utility patterns that handle async operations
5. **Keep patterns consistent** across your codebase for maintainability

---

## Conclusion

These patterns solve common React challenges and improve code quality. Use them as tools in your toolbox, selecting the right one for each situation rather than applying them universally.
