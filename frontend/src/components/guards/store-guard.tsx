import { routes } from '@/config/routes';
import type { ReactNode } from 'react';
import { useAppNavigation } from '@/hooks';
import { useMyStore } from '@/api/stores';
import { LoadingState } from '@/components/ui';
import { useLocation } from '@tanstack/react-router';

interface StoreGuardProps {
  children: ReactNode;
}

export const StoreGuard = ({ children }: StoreGuardProps) => {
  const navigate = useAppNavigation();
  const location = useLocation();
  const { data: store, isLoading: isStoreLoading, error: storeError } = useMyStore();

  const isOnSetupPage = location.pathname.startsWith(routes.to.store.setupStore());
  if (isStoreLoading) {
    return (
      <div className="flex h-screen items-center justify-center">
        <LoadingState type="spinner" size="lg" />
      </div>
    );
  }

  if (!store || storeError) {
    navigate.goTo({ to: routes.to.store.setupStore(), replace: true });
  }

  if (store && isOnSetupPage) {
    navigate.goTo({ to: routes.to.app.root(), replace: true });
  }
  return <>{children}</>;
};
