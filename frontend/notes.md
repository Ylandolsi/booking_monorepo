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
