/* eslint-disable @typescript-eslint/no-unused-vars */
import { ProductType } from '../types';
import type { Store, Product, CreateStoreInput, UpdateStoreInput, CreateProductInput } from '../types';

let mockStores: Store[] = [];
let mockProducts: Product[] = [];
let storeIdCounter = 1;
let productIdCounter = 1;

const initialStore: Store = {
  id: storeIdCounter++,
  userId: 1,
  title: 'Linki Store',
  slug: 'linki-store',
  description: 'Welcome to my Linki store! Here you can find all my digital products and book 1:1 sessions.',
  isPublished: true,
  step: 2,
  socialLinks: [
    { platform: 'twitter', url: 'https://twitter.com/linki' },
    { platform: 'instagram', url: 'https://instagram.com/linki' },
  ],
  products: [],
  createdAt: new Date().toISOString(),
  picture: {
    url: 'https://i.pravatar.cc/150?u=linki',
    altText: 'Linki Store Profile',
  },
};

mockStores.push(initialStore);

// Mock API implementations
export const mockGetMyStore = async (): Promise<Store> => {
  console.log('mock: getMyStore');
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (mockStores.length > 0) {
        resolve(mockStores[0]);
      } else {
        // Simulate a 404 Not Found error
        const error = new Response(null, { status: 404, statusText: 'Not Found' });
        reject(error);
      }
    }, 500);
  });
};

export const mockGetStoreBySlug = async (slug: string): Promise<Store> => {
  console.log(`mock: getStoreBySlug with slug: ${slug}`);
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const store = mockStores.find((s) => s.slug === slug);
      if (store) {
        resolve(store);
      } else {
        const error = new Response(null, { status: 404, statusText: 'Not Found' });
        reject(error);
      }
    }, 500);
  });
};

export const mockCreateStore = async (data: CreateStoreInput): Promise<Store> => {
  console.log('mock: createStore with data:', data);
  return new Promise((resolve) => {
    setTimeout(() => {
      const newStore: Store = {
        id: storeIdCounter++,
        userId: 1, // Assuming a logged-in user
        ...data,
        isPublished: true,
        step: 1,
        socialLinks: data.socialLinks || [],
        products: [],
        createdAt: new Date().toISOString(),
        picture: data.picture ? { url: URL.createObjectURL(data.picture) } : undefined,
      };
      mockStores = [newStore]; // Replace existing store for simplicity in this mock
      resolve(newStore);
    }, 500);
  });
};

export const mockUpdateStore = async (data: UpdateStoreInput): Promise<Store> => {
  console.log('mock: updateStore with data:', data);
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const storeIndex = mockStores.findIndex((s) => s.id === data.id);
      if (storeIndex !== -1) {
        const currentStore = mockStores[storeIndex];
        const { picture, ...restData } = data;

        const updatedStore = { ...currentStore, ...restData };

        if (picture instanceof File) {
          updatedStore.picture = { url: URL.createObjectURL(picture) };
        } else if (picture) {
          updatedStore.picture = picture;
        }

        mockStores[storeIndex] = updatedStore;
        resolve(updatedStore);
      } else {
        const error = new Response(null, { status: 404, statusText: 'Not Found' });
        reject(error);
      }
    }, 500);
  });
};

export const mockGetProducts = async (storeId: number): Promise<Product[]> => {
  console.log(`mock: getProducts for storeId: ${storeId}`);
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockProducts.filter((p) => p.storeId === storeId));
    }, 500);
  });
};

export const mockCreateProduct = async (storeId: number, data: CreateProductInput): Promise<Product> => {
  console.log(`mock: createProduct for storeId: ${storeId} with data:`, data);
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const store = mockStores.find((s) => s.id === storeId);
      if (!store) {
        const error = new Response(null, { status: 404, statusText: 'Store Not Found' });
        return reject(error);
      }

      const newProduct: Product = {
        id: productIdCounter++,
        storeId,
        storeSlug: store.slug,
        productSlug: data.title.toLowerCase().replace(/\s+/g, '-'),
        isPublished: true,
        displayOrder: store.products.length,
        createdAt: new Date().toISOString(),
        ...data,
        productType: data.productType as ProductType, // Cast because of discriminated union
        thumbnailUrl: data.thumbnail ? URL.createObjectURL(data.thumbnail) : undefined,
      };

      mockProducts.push(newProduct);
      store.products.push(newProduct);

      resolve(newProduct);
    }, 500);
  });
};
