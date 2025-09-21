// import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
// import { createStore, getMyStore, getStoreBySlug, updateStore, getProducts, createProduct } from '../api';
// import type { CreateProductInput, CreateStoreInput, UpdateStoreInput } from '../types';

// const STORE_QUERY_KEY = 'store';
// const PRODUCTS_QUERY_KEY = 'products';

// // Store hooks
// export const useMyStore = () => {
//   return useQuery({
//     queryKey: [STORE_QUERY_KEY, 'my-store'],
//     queryFn: getMyStore,
//   });
// };

// export const useStoreBySlug = (slug: string) => {
//   return useQuery({
//     queryKey: [STORE_QUERY_KEY, slug],
//     queryFn: () => getStoreBySlug(slug),
//     enabled: !!slug,
//   });
// };

// export const useCreateStore = () => {
//   const queryClient = useQueryClient();
//   return useMutation({
//     mutationFn: (data: CreateStoreInput) => createStore(data),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY] });
//     },
//   });
// };

// export const useUpdateStore = () => {
//   const queryClient = useQueryClient();
//   return useMutation({
//     mutationFn: (data: UpdateStoreInput) => updateStore(data),
//     onSuccess: (updatedStore) => {
//       if (updatedStore) {
//         queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY, 'my-store'] });
//         if (updatedStore.slug) {
//           queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY, updatedStore.slug] });
//         }
//       }
//     },
//   });
// };

// // Product hooks
// export const useProducts = (storeId: number) => {
//   return useQuery({
//     queryKey: [PRODUCTS_QUERY_KEY, storeId],
//     queryFn: () => getProducts(storeId),
//     enabled: !!storeId,
//   });
// };

// export const useCreateProduct = (storeId: number) => {
//   const queryClient = useQueryClient();
//   return useMutation({
//     mutationFn: (data: CreateProductInput) => createProduct(storeId, data),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: [PRODUCTS_QUERY_KEY, storeId] });
//     },
//   });
// };
