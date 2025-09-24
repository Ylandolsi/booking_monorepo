import { createFileRoute } from '@tanstack/react-router';

import { SetupStore } from '@/features/app/store';
import { ROUTE_PATHS } from '@/config';

export const Route = createFileRoute(ROUTE_PATHS.APP.STORE.SETUP_STORE)({
  component: SetupStore,
});
