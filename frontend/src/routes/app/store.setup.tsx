import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/store/setup')({
  component: RouteComponent,
});

function RouteComponent() {
  return <div className="inset-0 bg-black">"hhhh"</div>;
}
