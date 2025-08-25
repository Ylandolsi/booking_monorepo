import { ROUTE_PATHS } from '@/config';
import { createFileRoute } from '@tanstack/react-router';
import { MeetsPage } from '@/features/app/session/get';

export const Route = createFileRoute(ROUTE_PATHS.APP.MEETS.INDEX)({
  component: MeetsPage,
});
