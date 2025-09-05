## Notes on Intersection Observer and Wheel Event Handling

### Intersection Observer for Image Visibility

```ts
useEffect(() => {
  const observer = new IntersectionObserver(
    ([entry]) => {
      if (entry.isIntersecting) {
        setIsInView(true);
        observer.disconnect();
      }
    },
    { threshold: 0.1 },
  );

  if (imgRef.current) {
    observer.observe(imgRef.current);
  }

  return () => observer.disconnect();
}, []);
```

- **Purpose:** Detects when an image enters the viewport (at least 10% visible).
- **Action:** Sets `isInView` to `true` and disconnects the observer to avoid repeated triggers.

---

### Wheel Event Handling in Dialogs/Drawers

```ts
useEffect(() => {
  const handleWheel = (e: WheelEvent) => {
    if (hasOpenPopover) {
      const target = e.target as HTMLElement;
      const popoverElement = target.closest('[data-state="open"]');
      const isCommandContent =
        popoverElement &&
        popoverElement.querySelector('.max-h-\\[200px\\]')?.contains(target);

      if (isCommandContent) {
        // Prevent dialog from scrolling when interacting with command list
        e.stopPropagation();
      }
    }
  };

  if (open) {
    document.addEventListener('wheel', handleWheel, { capture: true });
  }

  return () => {
    document.removeEventListener('wheel', handleWheel, { capture: true });
  };
}, [open, hasOpenPopover]);
```

- By setting { capture: true }, the DrawerDialog’s event listener catches the wheel event during the capture phase, _before_ it reaches the MultiSelect or its scrollable content.
- This allows the DrawerDialog to inspect the event (e.g., check if it’s happening inside the dropdown’s scrollable area) and decide whether to stop it (e.stopPropagation()) or let it continue.
- **Purpose:** Prevents dialog/drawer from scrolling when a popover (like a select dropdown) is open and the user scrolls inside it.
- **Mechanism:** Stops event propagation if the wheel event is inside a command list within an open popover.

---

### Popover State Registration

```ts
const registerPopover = React.useCallback((isOpen: boolean) => {
  setHasOpenPopover(isOpen);
}, []);
```

- **Purpose:** Updates the state to track if a popover is open.
- **Usage:** Used to control scroll behavior in dialogs/drawers.

# Explanation of the DrawerDialog & MultiSelect Scrolling Fix Approach

The approach used to fix the scrolling issue between `DrawerDialog` and `MultiSelect` is a **context-based event coordination system**. Here's how it works:

## The Problem

When a `MultiSelect` dropdown is opened inside a `DrawerDialog`, the parent container's scroll handler was capturing wheel events before they could reach the dropdown's scrollable content, preventing proper scrolling within the dropdown.

## The Solution Approach

### 1. **Context Provider Pattern**

```tsx
// In drawer-dialog.tsx
const PopoverContext = React.createContext({
  registerPopover: (isOpen: boolean) => {},
});
```

The `DrawerDialog` creates a context to communicate with child components about popover states.

### 2. **Child Registration System**

```tsx
// In multi-select.tsx
const popoverContext = usePopoverContext?.();

React.useEffect(() => {
  if (popoverContext?.registerPopover) {
    popoverContext.registerPopover(isPopoverOpen);
  }
}, [isPopoverOpen, popoverContext]);
```

The `MultiSelect` component registers its popover state with the parent `DrawerDialog` whenever it opens/closes.

### 3. **State Tracking in Parent**

```tsx
// In drawer-dialog.tsx
const [hasOpenPopover, setHasOpenPopover] = React.useState(false);

const registerPopover = React.useCallback((isOpen: boolean) => {
  setHasOpenPopover(isOpen);
}, []);
```

The `DrawerDialog` tracks whether any child popover is currently open.

### 4. **Selective Event Handling**

```tsx
// In drawer-dialog.tsx
React.useEffect(() => {
  const handleWheel = (e: WheelEvent) => {
    if (hasOpenPopover) {
      const target = e.target as HTMLElement;
      const popoverElement = target.closest('[data-state="open"]');
      const isCommandContent =
        popoverElement &&
        popoverElement.querySelector('.max-h-\\[200px\\]')?.contains(target);

      if (isCommandContent) {
        e.stopPropagation(); // Let the dropdown handle its own scrolling
      }
    }
  };

  if (open) {
    document.addEventListener('wheel', handleWheel, { capture: true });
  }

  return () => {
    document.removeEventListener('wheel', handleWheel, { capture: true });
  };
}, [open, hasOpenPopover]);
```

The `DrawerDialog` only intervenes with wheel events when:

- A child popover is open (`hasOpenPopover` is true)
- The scroll event originates from within a dropdown's scrollable area

## How It Works Step by Step

1. **Initial State**: `DrawerDialog` opens, `hasOpenPopover` is `false`
2. **MultiSelect Opens**: `MultiSelect` calls `registerPopover(true)`, setting `hasOpenPopover` to `true`
3. **Event Interception**: `DrawerDialog` starts monitoring wheel events with capture phase
4. **Selective Handling**: When a wheel event occurs:
   - If it's inside the dropdown's scrollable area (`.max-h-[200px]`), stop propagation
   - If it's outside, let the event bubble normally
5. **Cleanup**: When `MultiSelect` closes, it calls `registerPopover(false)`, stopping the intervention

## Key Technical Details

### Event Capture vs Bubble

```tsx
document.addEventListener('wheel', handleWheel, { capture: true });
```

Uses **capture phase** to intercept events before they reach their target, allowing the parent to decide whether to let the child handle the event.

### DOM Navigation

```tsx
const popoverElement = target.closest('[data-state="open"]');
const isCommandContent = popoverElement
  ?.querySelector('.max-h-\\[200px\\]')
  ?.contains(target);
```

- `closest()` finds the nearest open popover ancestor
- `querySelector()` locates the scrollable content within that popover
- `contains()` checks if the event target is within the scrollable area

### Context Wrapping

```tsx
const wrappedChildren = (
  <PopoverContext.Provider value={{ registerPopover }}>
    {children}
  </PopoverContext.Provider>
);
```

All children are wrapped in the context provider, making the registration function available to any nested `MultiSelect` components.

```ts
Item['name'])


export type Item = {
  name:
    | 'Home'
    | 'Profile'
    | 'Bookings'
    | 'Search'
    | 'Notifications'
    | 'Settings'
    | 'Become Mentor';
  icon: JSX.Element;
  click: () => void;
  badge?: string;
};

```

````tsx
                <Checkbox
                  id={key}
                  checked={isCompleted === true}
                  disabled
                  className="data-[state=checked]:bg-green-600"
                />
                ```
````

<!-- const alertIconMap = {
default: AlertCircle,
destructive: XCircle,
success: CheckCircle,
warning: AlertTriangle,
info: Info,
} as const;
// {React.createElement(alertIconMap['success'])} -->

after each form this is a better way to handle the success/error

<!-- {becomeMentorMutation.isError && (
<Alert variant={'destructive'}>
{React.createElement(alertIconMap['destructive'])}
<AlertTitle>Error</AlertTitle>
<AlertDescription>
Failed to register as a mentor. Please try again.
</AlertDescription>
</Alert>
)}

      {becomeMentorMutation.isSuccess && (
        <Alert variant={'success'}>
          {React.createElement(alertIconMap['success'])}
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>
            Failed to register as a mentor. Please try again.
          </AlertDescription>
        </Alert>
      )} -->

ctrl + e : search for files
ctrl + tab : naviagte the open tabs
ctrl + alt + click : show the file or wher eits called
ctrl + < + w : to enable multicursor
ctrl + shift + p : toggle multicursor modfier to return to : ctrl + click open new tab

```ts
export function useMentorDetails(
  userSlug?: string | null,
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<Mentor, Error> {
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

card inside dialog is a good idea

`````
      {/* Current Status Card */}
      <Card className="mb-8">
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-4">
          <CardTitle className="flex items-center gap-2">
            <Award className="w-5 h-5 text-blue-600" />
            Your Mentor Profile
          </CardTitle>

          <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
            <DialogTrigger>
              <Button variant="outline" size="sm">
                <Edit3 className="w-4 h-4 mr-2" />
                Edit Profile
              </Button>
            </DialogTrigger>
            ````
`````

Great question! The reason for using `JSON.stringify` and then `JSON.parse` when setting `originalSchedule` is to create a **deep copy** of the `loadedSchedule` array.

### Why not just assign directly?

If you did:

```tsx
setOriginalSchedule(loadedSchedule);
```

Both `schedule` and `originalSchedule` would reference the **same array in memory**. Any changes to one would affect the other.

### Why use `JSON.parse(JSON.stringify(...))`?

This technique:

- Converts the object to a JSON string (`JSON.stringify`)
- Parses it back into a new object (`JSON.parse`)
- The result is a **completely new copy** with no shared references

### Gotcha

- This method works for simple objects/arrays, but won't copy functions, `Date` objects, or special types.

### Example

```tsx
const a = [{ x: 1 }];
const b = JSON.parse(JSON.stringify(a));
b[0].x = 2; // a[0].x is still 1
```

### In your code

This ensures that `originalSchedule` is a snapshot of the initial state, so you can compare or reset later without accidental mutation.

Let me know if you want to see alternative deep copy methods!

```ts
[field]: value means:
If field is 'start', it sets start: value.
If field is 'end', it sets end: value.

  const updateTimeRange = (
    day: DayOfWeek,
    rangeId: number,
    field: 'start' | 'end',
    value: string,
  ) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: ds.availabilityRanges.map(
        (r: AvailabilityRangeType) =>
          r.id === rangeId ? { ...r, [field]: value } : r,
      ),
    }));
```

```ts : group state in one single palce for better code
const [state, setState] = useState<BookingHookState>({
  selectedDate: undefined,
  selectedSlot: null,
  step: 'select',
  notes: '',
});
```

```ts
to show list in a good format , transparent a little bit when its not shown completly
            <div
              className="space-y-3 max-h-80 overflow-y-auto pr-2 pb-5"
              style={{
                maskImage:
                  'linear-gradient(to bottom, black 90%, transparent 100%)',
                WebkitMaskImage:
                  'linear-gradient(to bottom, black 90%, transparent 100%)',
              }}
            >
```

consided this appraoch for future :

```tsx
Package Approach for React Apps
src/
├── packages/
│   ├── ui/
│   │   ├── package.json
│   │   ├── Button/
│   │   ├── Modal/
│   │   └── index.ts
│   ├── hooks/
│   │   ├── package.json
│   │   └── src/
│   └── utils/
│       ├── package.json
│       └── src/
└── app/
    ├── pages/
    └── components/
json// packages/ui/package.json
{
  "name": "@myapp/ui",
  "exports": {
    "./button": "./Button/index.js",
    "./modal": "./Modal/index.js"
  }
}

```

```
  const [username, setUsername] = useState('')

      <input
            id="username"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            required
          />
```

abortController

```when loading a route with loaded , we can setup this
to show spinner when laoding but after some blank time that s the startyegy if tanstakc rotuer
export const Route = createFileRoute(ROUTE_PATHS.APP.PROFILE.USER)({
  component: RouteComponent,
  loader: async () => {
    // wait 5 seconds
    await new Promise<void>((resolve) => setTimeout(resolve, 5000));
    return undefined;
  },
  pendingComponent: PageLoading,
});
```

learn more about prefetech and prelaod ( tantaskc route r)

avoid bareel export and do package export https://claude.ai/share/3c6ded80-c263-4d76-a942-6b27263e1aea

from-blue-50 to-indigo-50

---

this pattern after post/patch update

```ts
const handlePayoutRequest = async (amount: number) => {
  try {
    await requestPayoutMutation.mutateAsync(amount);
    setPayoutSuccess(true);
    setIsPayoutDialogOpen(false);

    // Reset success message after 5 seconds
    setTimeout(() => {
      setPayoutSuccess(false); // show success state for 5 seconds
    }, 5000);
  } catch (error) {
    console.error('Payout request failed:', error);
  }
};

 {/* Success Alert */}
{payoutSuccess && (
  <Alert className="bg-green-50 border-green-200 dark:bg-green-950/20 dark:border-green-900">
    <AlertCircle className="h-4 w-4 text-green-600 dark:text-green-400" />
    <div className="ml-2">
      <p className="text-sm text-green-800 dark:text-green-200">
        <strong>Payout request submitted successfully!</strong>
      </p>
      <p className="text-xs text-green-700 dark:text-green-300 mt-1">
        Your payout request is being processed and will be completed
        within 3-5 business days.
      </p>
    </div>
  </Alert>
)}
```

SingleFlight

```ts
type AsyncFunction<V> = () => Promise<V>;

const pendingPromises: Map<string, Promise<any>> = new Map();

const run = async <V>(key: string, fn: AsyncFunction<V>): Promise<V> => {
  if (pendingPromises.has(key)) {
    return pendingPromises.get(key) as Promise<V>;
  }

  const promise = fn();
  pendingPromises.set(key, promise);

  try {
    const result = await promise;
    return result;
  } finally {
    pendingPromises.delete(key);
  }
};

export default {
  run,
};
```


  overrides?: Partial<UseQueryOptions<Array<Session>, unknown, Array<Session>>>,
