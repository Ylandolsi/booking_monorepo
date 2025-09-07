import { cn } from '@/lib/cn';
import logo from '@/assets/logo.svg';

export const Logo = ({ className }: { className?: string }) => {
  return (
    <img
      className={cn('h-12 w-auto', className)}
      style={{ filter: 'invert(0) sepia(1) saturate(10) hue-rotate(175deg)' }}
      src={logo}
      alt="Workflow"
    />
  );
};
