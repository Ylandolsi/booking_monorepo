import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/editstore')({
  component: RouteComponent,
});

function RouteComponent() {}
