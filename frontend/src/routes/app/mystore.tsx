import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/app/mystore')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/app/mystore"!</div>
}
