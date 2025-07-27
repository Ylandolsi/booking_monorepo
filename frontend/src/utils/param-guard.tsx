import type React from 'react';

type ParamGuardProps = {
  params: Record<string, string> | null;
  fallback?: React.ReactNode;
  children: React.ReactNode;
};

export function ParamGuard({ params, fallback, children }: ParamGuardProps) {
  if (!params) {
    return <>{fallback ?? <div>Missing requried parameters</div>}</>;
  }
  return <> {children}</>;
}
