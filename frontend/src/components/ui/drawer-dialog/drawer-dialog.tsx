import * as React from 'react';

import { cn } from '@/utils/cn';
import { useMediaQuery } from '@uidotdev/usehooks';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/drawer-dialog/dialog';
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from '@/components/ui/drawer-dialog/drawer';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';

export function DrawerDialog({
  trigger,
  title,
  description,
  children,
}: {
  trigger?: React.ReactNode;
  title?: string;
  description?: string;
  children?: React.ReactNode;
}) {
  const [open, setOpen] = React.useState(false);
  const isDesktop = useMediaQuery('(min-width: 768px)');

  if (isDesktop) {
    return (
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>
          {trigger || <Button variant="outline">Open Dialog</Button>}
        </DialogTrigger>
        <DialogContent className="border-border shadow-xs max-h-[80vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>{title}</DialogTitle>
            <DialogDescription>
              {description || "Click save when you're done."}
            </DialogDescription>
          </DialogHeader>
          {children || <ProfileForm />}
        </DialogContent>
      </Dialog>
    );
  }

  return (
    <Drawer open={open} onOpenChange={setOpen}>
      <DrawerTrigger asChild>
        {trigger || <Button variant="outline">Open Drawer</Button>}
      </DrawerTrigger>
      <DrawerContent className="px-5 max-h-[90vh] flex flex-col">
        <DrawerHeader className="text-left">
          <DrawerTitle>{title}</DrawerTitle>
          <DrawerDescription>
            {description || "Click save when you're done."}
          </DrawerDescription>
        </DrawerHeader>
        <div className="flex-1 overflow-y-auto">
          {/* Render children or default form */}
          {children || <ProfileForm className="px-4" />}
        </div>
        <DrawerFooter className="pt-10">
          <DrawerClose asChild>
            <Button variant="outline" className="z-10">
              Cancel
            </Button>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
}

// for demo purposes
function ProfileForm({ className }: React.ComponentProps<'form'>) {
  return (
    <form className={cn('grid items-start gap-6', className)}>
      <div className="grid gap-3">
        <Label htmlFor="email">Email</Label>
        <Input type="email" id="email" defaultValue="shadcn@example.com" />
      </div>
      <div className="grid gap-3">
        <Label htmlFor="username">Username</Label>
        <Input id="username" defaultValue="@shadcn" />
      </div>
      <Button type="submit">Save changes</Button>
    </form>
  );
}
