import type { ProductType } from '@/api/stores';
import { routes } from '@/config';
import { AddProductFlow } from '@/pages/store/private/products';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(routes.paths.APP.STORE.PRODUCT.INDEX)({
  component: AddProductFlow,
  // ?type=Session | ?type=DigitalDownload      &editSlug=product-slug
  validateSearch: (search) => ({
    type: search.type as ProductType | undefined,
    productSlug: search.productSlug as string | undefined,
    // email: search.email as string | undefined,
  }),
});
