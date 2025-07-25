import ReactDOM from 'react-dom/client';
import './index.css';
import { AppProvider } from './providers/app-provider';
import { createRouter, RouterProvider } from '@tanstack/react-router';
import { routeTree } from './routeTree.gen';

const router = createRouter({ routeTree });

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router;
  }
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  // <React.StrictMode>
  <AppProvider>
    <RouterProvider router={router} />
  </AppProvider>,
  // {/* </React.StrictMode>, */}
);
