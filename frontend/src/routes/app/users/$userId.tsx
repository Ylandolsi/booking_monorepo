import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/app/users/$userId')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/app/users/$userId"!</div>
}
