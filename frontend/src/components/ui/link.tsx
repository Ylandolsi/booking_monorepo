// import { Link as RouterLink, type LinkProps } from 'react-router';
import { Link as TanStackLink, type LinkProps as TanStackLinkProps } from "@tanstack/react-router";

import { cn } from '@/utils/cn';

export type LinkProps = {
    className?: string;
    children: React.ReactNode;
    target?: string;
} & TanStackLinkProps;

export const Link = ({ className, children, to, ...props }: LinkProps) => {
    return (
        <TanStackLink
            to={to}
            className={cn("text-slate-600 hover:text-slate-900", className)}
            {...props}
        >
            {children}
        </TanStackLink>
    );
};
