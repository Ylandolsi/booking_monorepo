import { PublicStorePreview } from '@/pages/store';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/(public)/store/$storeSlug/')({
  component: PublicStorePreview,
});
