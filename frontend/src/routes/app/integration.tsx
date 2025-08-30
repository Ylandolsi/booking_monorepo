import { IntegrationPage } from '@/features/app/integrations/integration-page';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/integration')({
  component: IntegrationPage,
});
