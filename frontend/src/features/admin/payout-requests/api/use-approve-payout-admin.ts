import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { AdminPayoutKeys } from './admin-payout-keys';

import type { ApprovePayoutRequest, ApprovePayoutAdminResponse } from '../types';

export { type ApprovePayoutAdminResponse } from '../types';

export const useApprovePayoutAdmin = () => {
  return useMutation({
    mutationFn: async (payoutId: number): Promise<ApprovePayoutAdminResponse> => {
      return await api.post<ApprovePayoutAdminResponse>(
        MentorshipEndpoints.Payouts.Admin.ApprovePayout,
        {
          PayoutId: payoutId,
        } satisfies ApprovePayoutRequest
      );
    },
    meta: {
      invalidatesQuery: [AdminPayoutKeys.allPayouts()],
      successMessage: 'Payout approved successfully',
      // errorMessage: 'Failed to approve payout',
    },
  });
};