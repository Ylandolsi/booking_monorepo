import { PublicStorePreview } from '@/features';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/(public)/store/$storeSlug/')({
  component: PublicStorePreview,
});
