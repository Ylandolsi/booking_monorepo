import { Button } from '@/components/ui';
import { googleOIDC } from '@/features/auth';
import type { User } from '@/types/api';
import { LazyImage } from '@/utils';
import { CheckCircle, ExternalLink, Calendar, Clock } from 'lucide-react';

export function IntegrateGoogle({ user }: { user?: User }) {
  const googleIntegrated = user?.integratedWithGoogle;

  return (
    <div
      className={`group relative overflow-hidden flex items-center gap-6 rounded-2xl shadow-sm border-2 transition-all duration-300 hover:shadow-lg hover:-translate-y-1 p-6 ${
        googleIntegrated 
          ? 'border-green-200 bg-gradient-to-br from-white via-green-50/30 to-green-100/60 hover:shadow-green-100' 
          : 'border-gray-200 bg-gradient-to-br from-white via-gray-50/30 to-red-50/60 hover:border-primary/30 hover:shadow-primary/10'
      }`}
    >
      {/* Background decoration */}
      <div className={`absolute top-0 right-0 w-32 h-32 rounded-full blur-3xl transition-opacity duration-300 ${
        googleIntegrated ? 'bg-green-200/30' : 'bg-red-200/20 group-hover:bg-primary/20'
      }`} />
      
      {/* Logo section with enhanced styling */}
      <div className="relative flex-shrink-0">
        <div className={`absolute inset-0 rounded-2xl transition-all duration-300 ${
          googleIntegrated ? 'bg-green-100/50' : 'bg-gray-100/50 group-hover:bg-primary/10'
        }`} />
        <LazyImage
          className="w-16 h-16 relative z-10 transition-transform duration-300 group-hover:scale-110"
          alt="Google Calendar"
          placeholder="/google-calendar.png"
          src="/google-calendar.png"
        />
        {googleIntegrated && (
          <div className="absolute -top-1 -right-1 z-20">
            <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center animate-pulse">
              <CheckCircle className="w-4 h-4 text-white" />
            </div>
          </div>
        )}
      </div>

      {/* Content section */}
      <div className="flex-1 min-w-0">
        <div className="flex items-start justify-between gap-4">
          <div className="flex-1 space-y-2">
            <div className="flex items-center gap-3">
              <h3 className="font-semibold text-xl text-gray-900 group-hover:text-primary transition-colors">
                Google Calendar
              </h3>
              {googleIntegrated && (
                <span className="inline-flex items-center gap-1 px-2 py-1 bg-green-100 text-green-700 text-xs font-medium rounded-full">
                  <CheckCircle className="w-3 h-3" />
                  Connected
                </span>
              )}
            </div>
            
            <p className="text-muted-foreground text-base">
              {googleIntegrated 
                ? 'Automatically sync your mentoring sessions with your Google Calendar' 
                : 'Schedule meetings directly on your calendar '
              }
            </p>
            
            {googleIntegrated && user?.googleEmail && (
              <div className="flex items-center gap-2 text-sm text-green-700 bg-green-50 px-3 py-1.5 rounded-lg w-fit">
                <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse" />
                <span className="font-medium">{user.googleEmail}</span>
              </div>
            )}
            
            {!googleIntegrated && (
              <div className="flex items-center gap-4 text-sm text-muted-foreground mt-3">
                <div className="flex items-center gap-1">
                  <Calendar className="w-4 h-4" />
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
                  ? 'bg-green-100 text-green-700 hover:bg-green-200 border-green-200' 
                  : 'bg-primary hover:bg-primary/90 text-white hover:shadow-lg hover:shadow-primary/25'
              }`}
              variant={googleIntegrated ? 'outline' : 'default'}
              disabled={!!googleIntegrated}
              onClick={async () => await googleOIDC()}
            >
              {googleIntegrated && (
                <CheckCircle className="w-4 h-4 mr-2" />
              )}
              {googleIntegrated ? 'Connected' : 'Connect Now'}
              {!googleIntegrated && (
                <ExternalLink className="w-4 h-4 ml-2 transition-transform duration-300 group-hover/btn:translate-x-1" />
              )}
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
