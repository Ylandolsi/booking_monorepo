import { cn } from '@/lib/cn';
// import logo from '@/assets/logo.svg';
import { LOGO_FULL } from '@/assets';

export const Logo = ({ className }: { className?: string }) => {
  return <img className={cn('h-8 w-auto object-contain object-top', className)} src={LOGO_FULL} alt="Workflow" />;
};
