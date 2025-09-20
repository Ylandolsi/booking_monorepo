import { cn } from '@/lib/cn';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
}

export function MobileContainer({ children, className }: MobileContainerProps) {
  return (
    <div className={cn('max-w-sm mx-auto bg-background rounded-xl shadow-lg overflow-hidden', 'min-h-screen flex flex-col', className)}>
      {children}
    </div>
  );
}
