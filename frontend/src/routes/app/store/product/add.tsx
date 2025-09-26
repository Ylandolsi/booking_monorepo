import type { ProductType } from '@/api/stores';
import { routes } from '@/config';
import { AddProductFlow } from '@/features/app/store/products';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(routes.paths.APP.STORE.PRODUCT.ADD_PRODUCT)({
  component: AddProductFlow,
  // ?type=Session | ?type=DigitalDownload      &editSlug=product-slug
  validateSearch: (search) => ({
    type: search.type as ProductType | undefined,
    editSlug: search.editSlug as string | undefined,
    // email: search.email as string | undefined,
  }),
});
