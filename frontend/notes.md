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
