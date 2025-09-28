import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api/api-client';
import { AdminPayoutKeys } from './admin-payout-keys';
import { CatalogEndpoints } from '@/lib';

type ApprovePayoutRequest = {
  PayoutId: number;
};
type PayoutRequestResponse = {
  payUrl: string;
};

export const useApprovePayoutAdmin = () => {
  return useMutation({
    mutationFn: async (payoutId: number): Promise<PayoutRequestResponse> => {
      return await api.post<PayoutRequestResponse>(CatalogEndpoints.Payouts.Admin.ApprovePayout, {
        PayoutId: payoutId,
      } satisfies ApprovePayoutRequest);
    },
    meta: {
      invalidatesQuery: [AdminPayoutKeys.allPayouts()],
      successMessage: 'Payout approved successfully',
      successAction: (data: PayoutRequestResponse) => {
        window.open(data.payUrl, '_blank');
      },
      // errorMessage: 'Failed to approve payout',
    },
  });
};
