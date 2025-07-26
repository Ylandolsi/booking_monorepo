import { ContentLayout } from '@/components/layouts';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <ContentLayout>
      <div className="flex h-screen p-4 bg-amber-100/20">
        <h1 className="text-2xl font-bold">Welcome 💫 </h1>
      </div>
    </ContentLayout>
  );
}
