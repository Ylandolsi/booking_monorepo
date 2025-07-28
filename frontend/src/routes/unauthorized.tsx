import { Unauthorized } from '@/components';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/unauthorized')({
  component: Unauthorized,
});
