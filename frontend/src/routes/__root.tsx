// eslint-disable-next-line check-file/filename-naming-convention
import { ErrorComponenet, MainErrorFallback } from '@/components';
import { createRootRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';

export const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools />
    </>
  ),
  notFoundComponent: () => <ErrorComponenet />,
  errorComponent: () => <MainErrorFallback />,
  // errorComponent: ({ error, reset }) => {
  //   return (
  //     <div>
  //       {error.message}
  //       <button
  //         onClick={() => {
  //           // Reset the router error boundary
  //           reset();
  //         }}
  //       >
  //         retry
  //       </button>
  //     </div>
  //   );
  // },
});
