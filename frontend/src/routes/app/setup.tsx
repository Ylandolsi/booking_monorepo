import { createFileRoute } from '@tanstack/react-router';

import { SetupStore } from '@/features/app/store';

export const Route = createFileRoute('/app/setup')({
  component: SetupStore,
});
