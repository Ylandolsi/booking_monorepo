import { createFileRoute } from '@tanstack/react-router';

import { EnhancedStoreBuilderDemo } from '@/components/store/enhanced-store-builder-demo';

export const Route = createFileRoute('/store-builder-enhanced')({
  component: EnhancedStoreBuilderPage,
});

function EnhancedStoreBuilderPage() {
  return <EnhancedStoreBuilderDemo />;
}
