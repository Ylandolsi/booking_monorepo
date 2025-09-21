import { cn } from '@/lib/cn';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
}

export function MobileContainer({ children, className }: MobileContainerProps) {
  return (
    <div style={{}} className={cn('bg-background flex h-full w-full flex-col overflow-y-auto p-4 text-center', className)}>
      {children}
    </div>
  );
}
