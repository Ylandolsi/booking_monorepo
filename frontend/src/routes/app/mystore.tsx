import { Card, CardContent } from '@/components';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { LazyImage } from '@/utils';
import { createFileRoute } from '@tanstack/react-router';
import { Calendar, Facebook, Globe, Instagram, Twitter, Youtube } from 'lucide-react';
import { IPhoneMockup } from 'react-device-mockup';

export const Route = createFileRoute('/app/mystore')({
  component: RouteComponent,
});
const socialPlatforms = [
  { key: 'instagram', label: 'Instagram', icon: Instagram },
  { key: 'twitter', label: 'Twitter', icon: Twitter },
  { key: 'facebook', label: 'Facebook', icon: Facebook },
  { key: 'youtube', label: 'YouTube', icon: Youtube },
  { key: 'website', label: 'Website', icon: Globe },
];
function RouteComponent() {
  return <div className="flex w-full flex-col items-start justify-around gap-8 md:flex-row"></div>;
}
