import { createFileRoute } from '@tanstack/react-router';
import { LinkiStorePage } from '@/features/store/components/pages/linki-store-page';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.LINKI)({
  component: LinkiStorePage,
});
