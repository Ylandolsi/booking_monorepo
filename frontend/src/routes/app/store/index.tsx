import { routes } from '@/config';
import { ModifyStore } from '@/features';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(routes.paths.APP.STORE.INDEX)({
  component: ModifyStore,
});
