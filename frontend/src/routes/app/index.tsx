import { createFileRoute, useLocation } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';
import { useEffect } from 'react';
import { toast } from 'sonner';

export const Route = createFileRoute(ROUTE_PATHS.APP.INDEX)({
  component: Index,
});

function Index() {
  const location = useLocation();
  const error = new URLSearchParams(location.search).get('error') ?? undefined;

  useEffect(() => {
    if (error) {
      toast.error(error);
    }
  }, [error]);

  return (
    <div className="flex h-screen p-4 ">
      <h1 className="text-2xl font-bold">Welcome 💫 </h1>
    </div>
  );
}
