## 📖 **Usage Patterns**

### **✅ Recommended (Refactored)**

```typescript
// Navigation Hook
const nav = useAppNavigation();
const handleClick = () => nav.goToLogin({ redirectTo: '/app' });

// Link Component
<Link to={routes.to.auth.login({ redirectTo: '/app' })} variant="primary">
  Login
</Link>
```

### **❌ Avoid (Old Patterns)**

```typescript
// Direct useNavigate (use useAppNavigation instead)
const navigate = useNavigate();
navigate({ to: '/auth/login?redirectTo=/app' });

// Hardcoded strings (use route builders)
<Link to="/auth/login">Login</Link>
window.location.href = '/app/bookings';

// Manual URL construction (use route builders)
const url = `/auth/login?redirectTo=${encodeURIComponent(path)}`;
```

---

## 🔧 **Quick Fixes for Remaining Components**

### **1. Auth Forms Pattern**

```typescript
// Add to imports
import { routes } from '@/config/routes';
import { useAppNavigation } from '@/hooks/use-navigation';

// Replace hardcoded links
<Link href={routes.to.auth.login()}>Login</Link>
<Link href={routes.to.auth.register()}>Register</Link>

// Replace navigation calls
const nav = useAppNavigation();
nav.goToLogin({ redirectTo: targetPath });
```

### **2. Error Components Pattern**

```typescript
// Replace useNavigate with useAppNavigation
const nav = useAppNavigation();
const handleGoHome = () => nav.goToHome();
const handleGoBack = () => nav.goBack();
```
