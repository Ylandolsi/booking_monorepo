import { Button } from '@/components/ui';
import { KonnectIntegrationDialog } from '@/features/app/integrations/components/konnect-dialog';
import type { User } from '@/types/api';
import { LazyImage } from '@/utils';
import { useState } from 'react';

export function IntegrateKonnect({ user }: { user?: User }) {
  const konnectIntegrated = !!user?.konnectWalletId?.trim();
  const [konnectDialogOpen, setKonnectDialogOpen] = useState<boolean>(false);

  return (
    <>
      <KonnectIntegrationDialog
        konnectDialogOpen={konnectDialogOpen}
        setKonnectDialogOpen={setKonnectDialogOpen}
      />
      <div
        className={`flex  items-center gap-4  rounded-2xl shadow-sm border-2 border-border p-4 bg-gradient-to-br ${konnectIntegrated ? `from-white to-green-50/60` : `from-white to-red-50/60`} `}
      >
        <LazyImage
          className="w-15 "
          alt="Konnect Network"
          placeholder="/konnect.svg"
          src="/konnect.svg"
        />
        <div className="flex justify-between gap-4 flex-1 flex-col sm:flex-row">
          <div className="flex flex-col ">
            <div className=" font-semibold text-xl">Konnect Network</div>
            <div className="text-muted-foreground">Receive money.</div>
            <div className="text-muted-foreground">
              {konnectIntegrated && user?.konnectWalletId}
            </div>
          </div>
          <Button
            className={`rounded-xl ${konnectIntegrated ? 'bg-accent text-foreground disabled hover:cursor-none' : ''}`}
            disabled={!!konnectIntegrated}
            onClick={() => setKonnectDialogOpen(true)}
          >
            {konnectIntegrated ? 'Integrated' : 'Integrate'}
          </Button>
        </div>
      </div>
    </>
  );
}
