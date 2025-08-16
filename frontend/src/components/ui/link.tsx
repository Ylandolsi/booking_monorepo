// import { Link as RouterLink, type LinkProps } from 'react-router';
import {
  Link as TanStackLink,
  type LinkProps as TanStackLinkProps,
} from '@tanstack/react-router';
import { cn } from '@/utils/cn';

export type LinkProps = {
  className?: string;
  children: React.ReactNode;
  target?: string;
  variant?: 'default' | 'primary' | 'secondary' | 'muted';
} & TanStackLinkProps;

export const Link = ({
  className,
  children,
  to,
  variant = 'default',
  ...props
}: LinkProps) => {
  const variantClasses = {
    default: 'text-slate-600 hover:text-slate-900',
    primary: 'text-blue-600 hover:text-blue-800',
    secondary: 'text-gray-600 hover:text-gray-800',
    muted: 'text-gray-500 hover:text-gray-700',
  };

  return (
    <TanStackLink
      to={to as any} // Temporarily bypass strict typing
      className={cn(variantClasses[variant], className)}
      {...props}
    >
      {children}
    </TanStackLink>
  );
};
