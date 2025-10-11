import { useIntegrateWithKonnect } from '@/api/stores/integrations/integrate-konnect-api';
import { Button, DrawerDialog, Input, Label } from '@/components';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import z from 'zod';
import { logger } from '@/lib';

export function KonnectIntegrationDialog({
  setKonnectDialogOpen,
  konnectDialogOpen,
}: {
  konnectDialogOpen: boolean;
  setKonnectDialogOpen: (b: boolean) => void;
}) {
  const integrateWithKonnectMutation = useIntegrateWithKonnect();

  const integrateWithKonnectSchema = z.object({
    walletId: z.string().min(1, 'WalletId is required'),
  });

  //prettier-ignore
  type IntegrateWithKonnectFormData = z.infer<typeof integrateWithKonnectSchema>;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<IntegrateWithKonnectFormData>({
    resolver: zodResolver(integrateWithKonnectSchema),
  });

  const onSubmit = async (data: IntegrateWithKonnectFormData) => {
    try {
      await integrateWithKonnectMutation.mutateAsync(data.walletId);
      reset();
      setKonnectDialogOpen(false); // Close dialog on success
    } catch (error) {
      // Error is already handled by the mutation's meta.errorMessage
      logger.error('Integration failed:', error);
    }
  };

  return (
    <DrawerDialog
      open={konnectDialogOpen}
      onOpenChange={setKonnectDialogOpen}
      title="Integrate With Konnect"
      description="Integrate your account with your Konnect Wallet."
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div className="space-y-2">
          <Label htmlFor="hourlyRate">Walled Id</Label>
          <Input
            id="hourlyRate"
            type="string"
            placeholder="5f7a209aeb3f76490ac4a3d1"
            {...register('walletId')}
            className={errors.walletId ? 'border-red-500' : ''}
          />
          {errors.walletId && <p className="text-sm text-red-600">{errors.walletId.message}</p>}
        </div>
        <Button type="submit" disabled={isSubmitting}>
          {' '}
          Integrate
        </Button>
      </form>
    </DrawerDialog>
  );
}
