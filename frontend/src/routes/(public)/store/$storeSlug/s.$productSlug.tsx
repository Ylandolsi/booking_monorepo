import { PublicStoreProductPreview } from '@/pages/store/public/session-product';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/(public)/store/$storeSlug/s/$productSlug')({
  component: PublicStoreProductPreview,
});
