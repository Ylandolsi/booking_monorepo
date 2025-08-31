import { useMutation, useQueryClient } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';

interface PayoutRequestData {
  Amount: number;
}

interface PayoutRequestResponse {
  success: boolean;
  message?: string;
}

export const useRequestPayout = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (amount: number): Promise<PayoutRequestResponse> => {
      const response = await api.post<PayoutRequestResponse>(
        MentorshipEndpoints.Payments.Payout,
        {
          Amount: Math.round(amount), // Backend expects amount in dollars as integer
        } satisfies PayoutRequestData
      );
      return response;
    },
    onSuccess: () => {
      // Invalidate related queries to refresh data
      queryClient.invalidateQueries({ queryKey: ['payout-history'] });
      queryClient.invalidateQueries({ queryKey: ['wallet'] });
    },
  });
};