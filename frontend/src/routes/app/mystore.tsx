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
  return (
    <div className="flex flex-col md:flex-row w-full   gap-8 items-start justify-around">
      <div className="flex flex-col w-full gap-5">
        {/* General Informatiosn */}
        <Card className="w-full mb-10">
          <CardContent>
            <div className="flex flex-col sm:flex-row">
              <LazyImage
                className="w-20 h-20 rounded-full ring-2 ring-primary/20 "
                src={FALLBACK_PROFILE_PICTURE}
                alt="profile-pic"
                placeholder={FALLBACK_PROFILE_PICTURE}
              />{' '}
              <div className="flex flex-col ml-4  ">
                <div>
                  <h2>Yassine landolsi</h2>
                  <p className="text-muted-foreground">@mohvmed_yassine</p>
                </div>
                <div className="flex justify-center gap-4 mt-auto">
                  {socialPlatforms.map(({ key, icon: Icon }) => (
                    <a
                      key={key}
                      href={'/test'}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="text-primary hover:text-accent transition-colors"
                    >
                      <Icon className="w-5 h-5" />
                    </a>
                  ))}
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
        <Card className="w-full">
          <CardContent>
            <div className="flex flex-col sm:flex-row">
              <LazyImage
                className="w-16 h-16 relative z-10 transition-transform duration-300 group-hover:scale-110"
                alt="Google Calendar"
                placeholder="/google-calendar.png"
                src="/google-calendar.png"
              />{' '}
              <div className="flex flex-col ml-4  ">
                <div>
                  <h2>1:1 coaching session </h2>
                  <p className="text-muted-foreground">50$</p>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
      <div className="flex-1 max-w-sm animate-in slide-in-from-right duration-700">
        <div className="flex justify-center">
          <IPhoneMockup screenWidth={320}>
            <div className="p-6 text-center bg-background min-h-full overflow-y-auto">
              <div className="space-y-3">
                <div className="bg-muted rounded-lg p-4 text-left border border-border">
                  <div className="flex items-center gap-2 mb-1">
                    <Calendar className="w-3 h-3 text-primary" />
                    <span className="text-xs text-muted-foreground">1:1 Coaching Call</span>
                  </div>
                  <div className="font-semibold text-foreground">Book a Call</div>
                  <div className="text-primary font-semibold">$99</div>
                </div>
                <div className="bg-muted rounded-lg p-4 text-left border border-border">
                  <div className="text-xs text-muted-foreground mb-1">Sample Product</div>
                  <div className="font-semibold text-foreground">Digital Guide</div>
                  <div className="text-primary font-semibold">$29</div>
                </div>
              </div>
            </div>
          </IPhoneMockup>
        </div>
      </div>
    </div>
  );
}
