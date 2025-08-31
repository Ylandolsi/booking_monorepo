import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/test/booking-demo')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/test/booking-demo"!</div>
}
