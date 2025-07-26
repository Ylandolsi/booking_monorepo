import * as React from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '../dialog';
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from './drawer';
import useMediaQuery from '@/hooks/use-media-query';

interface DrawerDialogProps {
  children: React.ReactNode;
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
  trigger?: React.ReactNode;
  title?: string;
  description?: string;
}

const PopoverContext = React.createContext({
  registerPopover: (isOpen: boolean) => {},
});

export function DrawerDialog({
  children,
  open: controlledOpen,
  onOpenChange: controlledOnOpenChange,
  trigger,
  title,
  description,
}: DrawerDialogProps) {
  const [internalOpen, setInternalOpen] = React.useState(false);
  const isDesktop = useMediaQuery('(min-width: 768px)');
  const [hasOpenPopover, setHasOpenPopover] = React.useState(false);

  // Use controlled state if provided, otherwise use internal state
  const open = controlledOpen !== undefined ? controlledOpen : internalOpen;
  const onOpenChange = controlledOnOpenChange || setInternalOpen;

  React.useEffect(() => {
    // Create a function to handle wheel events on the dialog/drawer content
    const handleWheel = (e: WheelEvent) => {
      if (hasOpenPopover) {
        // If a popover is open inside the dialog, check if it's part of a command list
        const target = e.target as HTMLElement;
        const popoverElement = target.closest('[data-state="open"]'); // Find the open popover
        const isCommandContent =
          popoverElement &&
          popoverElement.querySelector('.max-h-\\[200px\\]')?.contains(target); // Escape the class name

        if (isCommandContent) {
          // Let the command list handle its own scrolling
          // stop the scroll of the dialog and let the select handle its scroll !
          e.stopPropagation();
        }
      }
    };

    // Add the event listener when the dialog is open
    if (open) {
      document.addEventListener('wheel', handleWheel, { capture: true });
    }

    return () => {
      document.removeEventListener('wheel', handleWheel, { capture: true });
    };
  }, [open, hasOpenPopover]);

  const registerPopover = React.useCallback((isOpen: boolean) => {
    setHasOpenPopover(isOpen);
  }, []);

  const wrappedChildren = (
    <PopoverContext.Provider value={{ registerPopover }}>
      {children}
    </PopoverContext.Provider>
  );
  if (isDesktop) {
    return (
      <Dialog open={open} onOpenChange={onOpenChange}>
        {trigger && <DialogTrigger asChild>{trigger}</DialogTrigger>}
        <DialogContent className="sm:max-w-[600px] ">
          {(title || description) && (
            <DialogHeader>
              {title && <DialogTitle>{title}</DialogTitle>}
              {description && (
                <DialogDescription>{description}</DialogDescription>
              )}
            </DialogHeader>
          )}
          <div className="overflow-y-auto overflow-x-hidden max-h-[70vh] p-4">
            {wrappedChildren}
          </div>
        </DialogContent>
      </Dialog>
    );
  }

  return (
    <Drawer
      open={open}
      onOpenChange={onOpenChange}
      // Add these props to improve focus management
      shouldScaleBackground={false}
      preventScrollRestoration={true}
    >
      {trigger && <DrawerTrigger asChild>{trigger}</DrawerTrigger>}
      <DrawerContent>
        {(title || description) && (
          <DrawerHeader className="text-left">
            {title && <DrawerTitle>{title}</DrawerTitle>}
            {description && (
              <DrawerDescription>{description}</DrawerDescription>
            )}
          </DrawerHeader>
        )}
        <div className="px-4 pb-4 overflow-x-hidden overflow-y-auto max-h-[70vh] p-4">
          {wrappedChildren}
        </div>
      </DrawerContent>
    </Drawer>
  );
}

export const usePopoverContext = () => React.useContext(PopoverContext);
