import { PublicStoreProductPreview } from '@/pages/app/store/public/session-product';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/(public)/store/$storeSlug/s/$productSlug')({
  component: PublicStoreProductPreview,
});
