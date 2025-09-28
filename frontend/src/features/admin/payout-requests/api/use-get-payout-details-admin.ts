// import {
//   useQuery,
//   type UseQueryOptions,
//   type UseQueryResult,
// } from '@tanstack/react-query';
// import { AdminPayoutKeys } from './admin-payout-keys';
// import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints';
// import { api } from '@/lib';
// import type { AdminPayoutResponse } from './use-get-all-payouts-admin';

// const getPayoutDetailsAdmin = async (
//   payoutId: number,
// ): Promise<AdminPayoutResponse> => {
//   const endpoint = MentorshipEndpoints.Payouts.Admin.GetDetailled.replace(
//     '{payoutId}',
//     payoutId.toString(),
//   );
//   return await api.get<AdminPayoutResponse>(endpoint);
// };

// export const useGetPayoutDetailsAdmin = (
//   payoutId: number,
//   overrides?: Partial<UseQueryOptions<AdminPayoutResponse, Error>>,
// ): UseQueryResult<AdminPayoutResponse, Error> => {
//   const options: UseQueryOptions<AdminPayoutResponse, Error> = {
//     queryKey: AdminPayoutKeys.payoutById(payoutId),
//     queryFn: () => getPayoutDetailsAdmin(payoutId),
//     enabled: !!payoutId,
//     ...overrides,
//   };

//   return useQuery<AdminPayoutResponse, Error>(options);
// };
