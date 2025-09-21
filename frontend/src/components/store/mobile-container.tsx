import { cn } from '@/lib/cn';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
}

export function MobileContainer({ children, className }: MobileContainerProps) {
  return <div className={cn('flex flex-col h-full w-full', className)}>{children}</div>;
}
