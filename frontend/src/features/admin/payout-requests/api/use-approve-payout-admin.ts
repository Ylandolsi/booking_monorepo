import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { AdminPayoutKeys } from './admin-payout-keys';

type ApprovePayoutRequest =  {
  PayoutId: number;
}
type PayoutRequestResponse = {
  payUrl : string ;
}

export const useApprovePayoutAdmin = () => {
  return useMutation({
    mutationFn: async (payoutId: number): Promise<PayoutRequestResponse> => {
      return await api.post<PayoutRequestResponse>(
        MentorshipEndpoints.Payouts.Admin.ApprovePayout,
        {
          PayoutId: payoutId,
        } satisfies ApprovePayoutRequest
      );
    },
    meta: {
      invalidatesQuery: [AdminPayoutKeys.allPayouts()],
      successMessage: 'Payout approved successfully',
        successAction : (data : PayoutRequestResponse) => {
        window.open(data.payUrl, '_blank');
      },
      // errorMessage: 'Failed to approve payout',
    },
  });
};