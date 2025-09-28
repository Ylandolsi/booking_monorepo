import { Button } from '@/components/ui';
import { googleOIDC } from '@/api/auth';
import type { User } from '@/api/auth';
import { LazyImage } from '@/utils';
import { CheckCircle, ExternalLink, Calendar, Clock } from 'lucide-react';

export function IntegrateGoogle({ user }: { user?: User }) {
  const googleIntegrated = user?.integratedWithGoogle;

  return (
    <div
      className={`group relative flex items-center gap-6 overflow-hidden rounded-2xl border-2 p-6 shadow-sm transition-all duration-300 hover:-translate-y-1 hover:shadow-lg ${
        googleIntegrated
          ? 'border-green-200 bg-gradient-to-br from-white via-green-50/30 to-green-100/60 hover:shadow-green-100'
          : 'hover:border-primary/30 hover:shadow-primary/10 border-gray-200 bg-gradient-to-br from-white via-gray-50/30 to-red-50/60'
      }`}
    >
      {/* Background decoration */}
      <div
        className={`absolute top-0 right-0 h-32 w-32 rounded-full blur-3xl transition-opacity duration-300 ${
          googleIntegrated ? 'bg-green-200/30' : 'group-hover:bg-primary/20 bg-red-200/20'
        }`}
      />

      {/* Logo section with enhanced styling */}
      <div className="relative flex-shrink-0">
        <div
          className={`absolute inset-0 rounded-2xl transition-all duration-300 ${
            googleIntegrated ? 'bg-green-100/50' : 'group-hover:bg-primary/10 bg-gray-100/50'
          }`}
        />
        <LazyImage
          className="relative z-10 h-16 w-16 transition-transform duration-300 group-hover:scale-110"
          alt="Google Calendar"
          placeholder="/google-calendar.png"
          src="/google-calendar.png"
        />
        {googleIntegrated && (
          <div className="absolute -top-1 -right-1 z-20">
            <div className="flex h-6 w-6 animate-pulse items-center justify-center rounded-full bg-green-500">
              <CheckCircle className="h-4 w-4 text-white" />
            </div>
          </div>
        )}
      </div>

      {/* Content section */}
      <div className="min-w-0 flex-1">
        <div className="flex items-start justify-between gap-4">
          <div className="flex-1 space-y-2">
            <div className="flex items-center gap-3">
              <h3 className="group-hover:text-primary text-xl font-semibold text-gray-900 transition-colors">Google Calendar</h3>
              {googleIntegrated && (
                <span className="inline-flex items-center gap-1 rounded-full bg-green-100 px-2 py-1 text-xs font-medium text-green-700">
                  <CheckCircle className="h-3 w-3" />
                  Connected
                </span>
              )}
            </div>

            <p className="text-muted-foreground text-base">
              {googleIntegrated
                ? 'Automatically sync your mentoring sessions with your Google Calendar'
                : 'Schedule meetings directly on your calendar '}
            </p>

            {googleIntegrated && user?.googleEmail && (
              <div className="flex w-fit items-center gap-2 rounded-lg bg-green-50 px-3 py-1.5 text-sm text-green-700">
                <div className="h-2 w-2 animate-pulse rounded-full bg-green-500" />
                <span className="font-medium">{user.googleEmail}</span>
              </div>
            )}

            {!googleIntegrated && (
              <div className="text-muted-foreground mt-3 flex items-center gap-4 text-sm">
                <div className="flex items-center gap-1">
                  <Calendar className="h-4 w-4" />
                  <span>Auto-sync events</span>
                </div>
              </div>
            )}
          </div>

          {/* Action button */}
          <div className="flex-shrink-0">
            <Button
              className={`group/btn relative overflow-hidden transition-all duration-300 ${
                googleIntegrated
                  ? 'border-green-200 bg-green-100 text-green-700 hover:bg-green-200'
                  : 'bg-primary hover:bg-primary/90 hover:shadow-primary/25 text-white hover:shadow-lg'
              }`}
              variant={googleIntegrated ? 'outline' : 'default'}
              disabled={!!googleIntegrated}
              onClick={async () => await googleOIDC()}
            >
              {googleIntegrated && <CheckCircle className="mr-2 h-4 w-4" />}
              {googleIntegrated ? 'Connected' : 'Connect Now'}
              {!googleIntegrated && <ExternalLink className="ml-2 h-4 w-4 transition-transform duration-300 group-hover/btn:translate-x-1" />}
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
