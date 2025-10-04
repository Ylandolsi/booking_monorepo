import { ROUTE_PATHS } from '@/config';
import { createFileRoute } from '@tanstack/react-router';
import { SessionBookedCalendar } from '@/pages/app/store/products/get-all-sessions/get-all-sessions-page';

export const Route = createFileRoute(ROUTE_PATHS.APP.MEETS.INDEX)({
  component: SessionBookedCalendar,
});
