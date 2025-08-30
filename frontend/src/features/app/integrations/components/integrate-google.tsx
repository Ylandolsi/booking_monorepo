import { Button } from '@/components/ui';
import { googleOIDC } from '@/features/auth';
import type { User } from '@/types/api';
import { LazyImage } from '@/utils';

export function IntegrateGoogle({ user }: { user?: User }) {
  const googleIntegrated = user?.integratedWithGoogle;

  return (
    <div
      className={`flex  items-center gap-4  rounded-2xl shadow-sm border-2 border-border p-4 bg-gradient-to-br ${googleIntegrated ? `from-white to-green-50/60` : `from-white to-red-50/60`} `}
    >
      <LazyImage
        className="w-15 "
        alt="Google Calendar"
        placeholder="/google-calendar.png"
        src="/google-calendar.png"
      />
      <div className="flex justify-between gap-4 flex-1 flex-col sm:flex-row">
        <div className="flex flex-col ">
          <div className=" font-semibold text-xl">Google Calendar</div>
          <div className="text-muted-foreground">
            Schedule meetings on your calendar.
          </div>
          <div className="text-muted-foreground">
            {googleIntegrated && user.googleEmail}
          </div>
        </div>
        <Button
          className={`rounded-xl ${googleIntegrated ? 'bg-accent text-foreground disabled hover:cursor-none' : ''}`}
          disabled={!!googleIntegrated}
          onClick={async () => await googleOIDC()}
        >
          {googleIntegrated ? 'Integrated' : 'Integrate'}
        </Button>
      </div>
    </div>
  );
}
