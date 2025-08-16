import { ContentLayout } from '@/components/layouts';
import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute(ROUTE_PATHS.APP.INDEX)({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <div className="flex h-screen p-4 ">
        <h1 className="text-2xl font-bold">Welcome 💫 </h1>
      </div>
    </ContentLayout>
  );
}
