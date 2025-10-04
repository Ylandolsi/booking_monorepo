import { ROUTE_PATHS } from '@/config';
import { IntegrationPage } from '@/pages/app/store/integrations/integration-page';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(ROUTE_PATHS.APP.PROFILE.INTEGRATIONS)({
  component: IntegrationPage,
});
