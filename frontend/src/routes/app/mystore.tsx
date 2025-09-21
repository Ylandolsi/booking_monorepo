import { ModifyStore } from '@/features';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/mystore')({
  component: ModifyStore,
});
