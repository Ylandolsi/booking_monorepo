// /**
//  * React Query hooks for session products
//  */

// import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
// import {
//   createSessionProduct,
//   updateSessionProduct,
//   getSessionProduct,
//   getBookedSessions,
//   getDailyAvailability,
//   getMonthlyAvailability,
//   bookSession,
//   validateCreateSessionProductInput,
//   validateBookSessionRequest,
// } from '../products-api';
// import type { CreateSessionProductInput, UpdateSessionProductInput, BookSessionRequest } from '../products-api';
// import { productKeys, orderKeys, PRODUCT_QUERY_KEY } from '../../stores-keys';

// // ===== CREATE SESSION PRODUCT =====

// export const useCreateSessionProduct = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (data: CreateSessionProductInput) => createSessionProduct(data),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: productKeys.all });
//       queryClient.invalidateQueries({ queryKey: productKeys.sessions() });
//     },
//     meta: {
//       successMessage: 'Session product created successfully!',
//     },
//   });
// };

// // ===== UPDATE SESSION PRODUCT =====

// export const useUpdateSessionProduct = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (data: UpdateSessionProductInput) => updateSessionProduct(data),
//     onSuccess: (_, variables) => {
//       queryClient.invalidateQueries({ queryKey: productKeys.all });
//       queryClient.invalidateQueries({ queryKey: productKeys.sessions() });
//       queryClient.invalidateQueries({ queryKey: productKeys.session(variables.productSlug) });
//     },
//     meta: {
//       successMessage: 'Session product updated successfully!',
//     },
//   });
// };

// // ===== GET SESSION PRODUCT =====

// export const useSessionProduct = (productSlug: string, enabled = true) => {
//   return useQuery({
//     queryKey: productKeys.session(productSlug),
//     queryFn: () => getSessionProduct(productSlug),
//     enabled: enabled && !!productSlug,
//     staleTime: 5 * 60 * 1000, // 5 minutes
//   });
// };

// // ===== GET BOOKED SESSIONS =====

// export const useBookedSessions = () => {
//   return useQuery({
//     queryKey: productKeys.sessions(),
//     queryFn: () => getBookedSessions(),
//     staleTime: 30 * 1000, // 30 seconds
//   });
// };

// // ===== AVAILABILITY HOOKS =====

// export const useDailyAvailability = (productSlug: string, date: string, enabled = true) => {
//   return useQuery({
//     queryKey: productKeys.availability(productSlug, date),
//     queryFn: () => getDailyAvailability(productSlug, date),
//     enabled: enabled && !!productSlug && !!date,
//     staleTime: 60 * 1000, // 1 minute
//     retry: false, // Don't retry availability checks
//   });
// };

// export const useMonthlyAvailability = (productSlug: string, year: number, month: number, timeZoneId: string = 'Africa/Tunis', enabled = true) => {
//   return useQuery({
//     queryKey: productKeys.availability(productSlug, `${year}-${month}`),
//     queryFn: () => getMonthlyAvailability(productSlug, year, month, timeZoneId),
//     enabled: enabled && !!productSlug && !!year && !!month,
//     staleTime: 5 * 60 * 1000, // 5 minutes
//   });
// };

// // ===== BOOKING HOOK =====

// export const useBookSession = (productSlug: string) => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (data: BookSessionRequest) => bookSession(productSlug, data),
//     onSuccess: () => {
//       // Invalidate availability queries to reflect the new booking
//       queryClient.invalidateQueries({
//         queryKey: [PRODUCT_QUERY_KEY, 'sessions', productSlug, 'availability'],
//       });
//       // Invalidate booked sessions for the store owner
//       queryClient.invalidateQueries({ queryKey: productKeys.sessions() });
//       // Invalidate orders
//       queryClient.invalidateQueries({ queryKey: orderKeys.all });
//     },
//     meta: {
//       successMessage: 'Session booked successfully!',
//     },
//   });
// };

// // ===== RE-EXPORTS =====

// export {
//   createSessionProduct,
//   updateSessionProduct,
//   getSessionProduct,
//   getBookedSessions,
//   getDailyAvailability,
//   getMonthlyAvailability,
//   bookSession,
//   validateCreateSessionProductInput,
//   validateBookSessionRequest,
// };

// export type { CreateSessionProductInput, UpdateSessionProductInput, BookSessionRequest };
