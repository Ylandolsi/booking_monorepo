import { routes } from '@/config';
import { AddProductFlow } from '@/features/app/store/products';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(routes.paths.APP.STORE.PRODUCT.ADD_PRODUCT)({
  component: AddProductFlow,
});
