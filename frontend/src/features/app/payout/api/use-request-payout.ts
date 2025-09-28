import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api/api-client';
import { PayoutKeys } from '@/features/app/payout/api/payout-keys';
import { WalletKeys } from '@/api/stores';
import { CatalogEndpoints } from '@/lib';

type PayoutRequestData = {
  Amount: number;
};

export const useRequestPayout = () => {
  return useMutation({
    mutationFn: async (amount: number): Promise<void> => {
      await api.post<void>(CatalogEndpoints.Payouts.Payout, {
        Amount: amount,
      } satisfies PayoutRequestData);
    },
    meta: {
      // TODO : invalidate mentor-data-slug key
      invalidatesQuery: [PayoutKeys.history(), WalletKeys.wallet()],
      successMessage: 'Payout requested successfully ',
      // errorMessage: 'Failed to request to a payout',
    },
  });
};
