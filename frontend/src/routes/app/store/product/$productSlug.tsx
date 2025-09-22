import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/app/store/product/$productSlug')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/app/store/product/$product-slug"!</div>
}
