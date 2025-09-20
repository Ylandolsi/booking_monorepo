import { createFileRoute } from '@tanstack/react-router';import { createFileRoute } from '@tanstack/react-router'

import { EnhancedStoreBuilderDemo } from '@/components/store/enhanced-store-builder-demo';

export const Route = createFileRoute('/store-builder-enhanced-v2')({

export const Route = createFileRoute('/store-builder-enhanced')({  component: RouteComponent,

  component: EnhancedStoreBuilderPage,})

});

function RouteComponent() {

function EnhancedStoreBuilderPage() {  return <div>Hello "/store-builder-enhanced-v2"!</div>

  return <EnhancedStoreBuilderDemo />;}

}