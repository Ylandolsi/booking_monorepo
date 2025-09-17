import { mockCreateProduct, mockCreateStore, mockGetMyStore, mockGetProducts, mockGetStoreBySlug, mockUpdateStore } from './_mocks';

// Use mocks instead of real API calls
export const getMyStore = mockGetMyStore;
export const getStoreBySlug = mockGetStoreBySlug;
export const createStore = mockCreateStore;
export const updateStore = mockUpdateStore;
export const getProducts = mockGetProducts;
export const createProduct = mockCreateProduct;
