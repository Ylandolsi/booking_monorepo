import { createFileRoute } from '@tanstack/react-router';
import { StoreBuilderDemo } from '@/components/store/store-builder-demo';

export const Route = createFileRoute('/store-builder')({
  component: StoreBuilderPage,
});

function StoreBuilderPage() {
  return <StoreBuilderDemo />;
}
