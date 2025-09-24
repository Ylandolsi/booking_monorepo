import { routes } from '@/config/routes';
import type { ReactNode } from 'react';
import { useAppNavigation } from '@/hooks';
import { useMyStore } from '@/api/stores';
import { LoadingState } from '@/components/ui';

interface StoreGuardProps {
  children: ReactNode;
}

export const StoreGuard = ({ children }: StoreGuardProps) => {
  const navigate = useAppNavigation();
  const { data: store, isLoading: isStoreLoading, error: storeError } = useMyStore();
  console.log('StoreGuard store:', store);
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

  if (store) {
    navigate.goTo({ to: routes.to.app.root(), replace: true });
  }
  return <>{children}</>;
};
