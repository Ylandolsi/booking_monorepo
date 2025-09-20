import { cn } from '@/lib/cn';

interface ScrollableContentProps {
  children: React.ReactNode;
  className?: string;
}

export function ScrollableContent({ children, className }: ScrollableContentProps) {
  return <div className={cn('flex-1 overflow-y-auto p-4 space-y-4', className)}>{children}</div>;
}
