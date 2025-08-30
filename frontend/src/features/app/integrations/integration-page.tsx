import { Button } from '@/components';
import { LazyImage } from '@/utils';

export function IntegrationPage() {
  return (
    <>
      <div className="font-bold text-2xl pb-10">Integrations</div>
      <div className="flex flex-col gap-7">
        <div className="flex  items-center gap-4  rounded-2xl shadow-sm border-2 border-border p-4 bg-gradient-to-r from-white to-red-50">
          <LazyImage
            className="w-15 "
            alt="Google Calendar"
            placeholder="/google-calendar.png"
            src="/google-calendar.png"
          />
          <div className="flex flex-col ">
            <div className=" font-semibold text-xl">Google Calendar</div>
            <div className="text-muted-foreground">
              Schedule meetings on your calendar.
            </div>
            <div className="text-muted-foreground">yesslandolsi@gmail.com</div>
          </div>
          <Button className="ml-auto rounded-xl ">Integrate</Button>
        </div>
        <div className="flex items-center gap-4 rounded-2xl shadow-sm border-2 border-border p-4 bg-gradient-to-r from-white to-green-50">
          <LazyImage
            className="w-15 "
            alt="Konnect Network"
            placeholder="/konnect.svg"
            src="/konnect.svg"
          />
          <div className="flex flex-col ">
            <div className=" font-semibold text-xl">Konnect Network</div>
            <div className="text-muted-foreground">Receive money.</div>
            <div className="text-muted-foreground">
              5f7a*****************3d1
            </div>
          </div>
          <Button className="ml-auto rounded-xl ">Integrate</Button>
        </div>
      </div>
    </>
  );
}
