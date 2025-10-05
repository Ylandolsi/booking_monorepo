import DataTableDemo from '@/pages/app/store/private/orders/orders-page';
import { createFileRoute } from '@tanstack/react-router';
export const Route = createFileRoute('/app/orders')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="p-4">
      <h1 className="mb-4 text-2xl font-bold">Orders History</h1>
      <DataTableDemo />
    </div>
  );
}
