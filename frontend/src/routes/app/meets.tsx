import { ROUTE_PATHS } from '@/config';
import { SessionBookedCalendar } from '@/pages/store/private/products/get-all-sessions';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute(ROUTE_PATHS.APP.MEETS.INDEX)({
  component: SessionBookedCalendar,
});
