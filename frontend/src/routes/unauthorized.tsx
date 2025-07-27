import { createFileRoute } from '@tanstack/react-router';
import { UnauthorizedPage } from '@/components/pages/unauthorized';

export const Route = createFileRoute('/unauthorized')({
  component: UnauthorizedPage,
});
