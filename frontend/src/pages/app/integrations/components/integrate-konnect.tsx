import { Button } from '@/components/ui';
import { KonnectIntegrationDialog } from '@/pages/app/integrations/components/konnect-dialog';
import type { User } from '@/api/auth';
import { CheckCircle, ExternalLink, Wallet, Shield, Banknote } from 'lucide-react';
import { useState } from 'react';

export function IntegrateKonnect({ user }: { user?: User }) {
  const konnectIntegrated = !!user?.konnectWalletId?.trim();
  const [konnectDialogOpen, setKonnectDialogOpen] = useState<boolean>(false);

  return (
    <>
      <KonnectIntegrationDialog konnectDialogOpen={konnectDialogOpen} setKonnectDialogOpen={setKonnectDialogOpen} />
      <div
        className={`group relative flex items-center gap-6 overflow-hidden rounded-2xl border-2 p-6 shadow-sm transition-all duration-300 hover:-translate-y-1 hover:shadow-lg ${
          konnectIntegrated
            ? 'border-green-200 bg-gradient-to-br from-white via-green-50/30 to-green-100/60 hover:shadow-green-100'
            : 'hover:border-primary/30 hover:shadow-primary/10 to-destructive/10 from-background border-gray-200 bg-gradient-to-br'
        }`}
      >
        {/* Background decoration */}
        <div
          className={`absolute top-0 right-0 h-32 w-32 rounded-full blur-3xl transition-opacity duration-300 ${
            konnectIntegrated ? 'bg-green-200/30' : 'group-hover:bg-primary/20 bg-orange-200/20'
          }`}
        />

        {/* Logo section with enhanced styling */}
        <div className="relative flex-shrink-0">
          <div className={`absolute inset-0 rounded-2xl transition-all duration-300`} />
          <img
            className="relative z-10 h-16 w-16 rounded-sm bg-white p-2 transition-transform duration-300 group-hover:scale-110"
            alt="Konnect Network"
            src="/konnect.svg"
          />
          {konnectIntegrated && (
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
                <h3 className="group-hover:text-primary text-xl font-semibold transition-colors">Konnect Network</h3>
                {konnectIntegrated && (
                  <span className="inline-flex items-center gap-1 rounded-full bg-green-100 px-2 py-1 text-xs font-medium text-green-700">
                    <CheckCircle className="h-3 w-3" />
                    Connected
                  </span>
                )}
              </div>

              <p className="text-muted-foreground text-base">
                {konnectIntegrated
                  ? 'Receive secure payments directly to your Konnect wallet'
                  : 'Connect your Konnect wallet to receive payments seamlessly from clients'}
              </p>

              {konnectIntegrated && user?.konnectWalletId && (
                <div className="flex w-fit items-center gap-2 rounded-lg bg-green-50 px-3 py-1.5 text-sm text-green-700">
                  <div className="h-2 w-2 animate-pulse rounded-full bg-green-500" />
                  <Wallet className="h-3 w-3" />
                  <span className="font-mono font-medium">{user.konnectWalletId}</span>
                </div>
              )}

              {!konnectIntegrated && (
                <div className="text-muted-foreground mt-3 flex items-center gap-4 text-sm">
                  <div className="flex items-center gap-1">
                    <Banknote className="h-4 w-4" />
                    <span>Instant payments</span>
                  </div>
                  <div className="flex items-center gap-1">
                    <Shield className="h-4 w-4" />
                    <span>Secure transactions</span>
                  </div>
                </div>
              )}
            </div>

            {/* Action button */}
            <div className="flex-shrink-0">
              <Button
                className={`group/btn relative overflow-hidden transition-all duration-300 dark:text-white ${
                  konnectIntegrated
                    ? 'border-green-200 bg-green-100 text-green-700 hover:bg-green-200'
                    : 'bg-primary hover:bg-primary/90 hover:shadow-primary/25 text-white hover:shadow-lg'
                }`}
                variant={konnectIntegrated ? 'outline' : 'default'}
                disabled={!!konnectIntegrated}
                onClick={() => setKonnectDialogOpen(true)}
              >
                {konnectIntegrated && <CheckCircle className="mr-2 h-4 w-4" />}
                {konnectIntegrated ? 'Connected' : 'Connect Wallet'}
                {!konnectIntegrated && <ExternalLink className="ml-2 h-4 w-4 transition-transform duration-300 group-hover/btn:translate-x-1" />}
              </Button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
