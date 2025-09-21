/**
 * Complete Stores API Implementation
 * Replaces mock implementations with real backend integration
 */

import { api } from '@/lib/api/api-client';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { toFormData, validateFile } from '@/types/catalog-api';
import type { CreateStoreResponse, StoreResponse, PublicStoreResponse } from '@/types/catalog-api';

// ===== CREATE STORE =====

export interface CreateStoreInput {
  title: string;
  slug: string;
  description?: string;
  picture?: File;
  socialLinks?: Array<{ platform: string; url: string }>;
}

export const createStore = async (data: CreateStoreInput): Promise<CreateStoreResponse> => {
  // Validate the picture file if provided
  if (data.picture) {
    const validation = validateFile(data.picture, {
      maxSizeInMB: 5,
      allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
      required: false,
    });

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = toFormData({
    title: data.title,
    slug: data.slug,
    file: data.picture || new File([], ''), // Backend expects a file field
    description: data.description || '',
    socialLinks: data.socialLinks || [],
  });

  try {
    const response = await api.post<CreateStoreResponse>(CatalogEndpoints.Stores.Create, formData);

    return response;
  } catch (error) {
    console.error('Error creating store:', error);
    throw error;
  }
};

// ===== GET MY STORE =====

export const getMyStore = async (): Promise<StoreResponse> => {
  try {
    const response = await api.get<StoreResponse>(CatalogEndpoints.Stores.GetMy);
    return response;
  } catch (error) {
    console.error('Error fetching my store:', error);
    throw error;
  }
};

// ===== GET PUBLIC STORE =====

export const getPublicStore = async (slug: string): Promise<PublicStoreResponse> => {
  try {
    const response = await api.get<PublicStoreResponse>(CatalogEndpoints.Stores.GetPublic(slug));
    return response;
  } catch (error) {
    console.error(`Error fetching public store with slug ${slug}:`, error);
    throw error;
  }
};

// ===== UPDATE STORE =====

export interface UpdateStoreInput {
  title: string;
  slug: string;
  description?: string;
  socialLinks?: Array<{ platform: string; url: string }>;
}

export const updateStore = async (data: UpdateStoreInput): Promise<CreateStoreResponse> => {
  try {
    const response = await api.put<CreateStoreResponse>(CatalogEndpoints.Stores.Update, {
      title: data.title,
      slug: data.slug,
      description: data.description || '',
      socialLinks: data.socialLinks || [],
    });

    return response;
  } catch (error) {
    console.error('Error updating store:', error);
    throw error;
  }
};

// ===== UPDATE STORE PICTURE =====

export interface UpdateStorePictureInput {
  picture: File;
}

export const updateStorePicture = async (data: UpdateStorePictureInput): Promise<CreateStoreResponse> => {
  // Validate the picture file
  const validation = validateFile(data.picture, {
    maxSizeInMB: 5,
    allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
    required: true,
  });

  if (!validation.isValid) {
    throw new Error(validation.error || 'Invalid file');
  }

  // Create FormData for the request
  const formData = toFormData({
    file: data.picture,
  });

  try {
    const response = await api.patch<CreateStoreResponse>(CatalogEndpoints.Stores.UpdatePicture, formData);

    return response;
  } catch (error) {
    console.error('Error updating store picture:', error);
    throw error;
  }
};

// ===== CHECK SLUG AVAILABILITY =====

export interface SlugAvailabilityResponse {
  isAvailable: boolean;
  slug: string;
}

export const checkSlugAvailability = async (slug: string): Promise<SlugAvailabilityResponse> => {
  try {
    const response = await api.get<SlugAvailabilityResponse>(CatalogEndpoints.Stores.CheckSlugAvailability(slug));
    return response;
  } catch (error) {
    console.error(`Error checking slug availability for ${slug}:`, error);
    throw error;
  }
};

// ===== VALIDATION HELPERS =====

/**
 * Validate store creation input
 */
export function validateCreateStoreInput(data: CreateStoreInput): { isValid: boolean; errors: string[] } {
  const errors: string[] = [];

  // Title validation
  if (!data.title?.trim()) {
    errors.push('Store title is required');
  } else if (data.title.length > 100) {
    errors.push('Store title cannot exceed 100 characters');
  }

  // Slug validation
  if (!data.slug?.trim()) {
    errors.push('Store slug is required');
  } else if (data.slug.length > 50) {
    errors.push('Store slug cannot exceed 50 characters');
  } else if (!/^[a-z0-9-]+$/.test(data.slug)) {
    errors.push('Store slug can only contain lowercase letters, numbers, and hyphens');
  }

  // Description validation
  if (data.description && data.description.length > 1000) {
    errors.push('Store description cannot exceed 1000 characters');
  }

  // Picture validation
  if (data.picture) {
    const fileValidation = validateFile(data.picture, {
      maxSizeInMB: 5,
      allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
      required: false,
    });

    if (!fileValidation.isValid) {
      errors.push(fileValidation.error || 'Invalid picture file');
    }
  }

  // Social links validation
  if (data.socialLinks) {
    data.socialLinks.forEach((link, index) => {
      if (!link.platform?.trim()) {
        errors.push(`Social link ${index + 1}: Platform is required`);
      }
      if (!link.url?.trim()) {
        errors.push(`Social link ${index + 1}: URL is required`);
      } else {
        try {
          new URL(link.url);
        } catch {
          errors.push(`Social link ${index + 1}: Invalid URL format`);
        }
      }
    });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
}

/**
 * Validate update store input
 */
export function validateUpdateStoreInput(data: UpdateStoreInput): { isValid: boolean; errors: string[] } {
  // Reuse most validations from create, but picture is not included in update
  const createData: CreateStoreInput = {
    title: data.title,
    slug: data.slug,
    description: data.description,
    socialLinks: data.socialLinks,
    // No picture in update
  };

  return validateCreateStoreInput(createData);
}

// ===== ERROR HANDLING HELPERS =====

/**
 * Check if error is a specific store-related error
 */
export function isStoreError(error: any, type: 'slug-taken' | 'not-found' | 'already-exists'): boolean {
  if (!error?.message) return false;

  const message = error.message.toLowerCase();

  switch (type) {
    case 'slug-taken':
      return message.includes('slug') && (message.includes('taken') || message.includes('exists') || message.includes('unavailable'));
    case 'not-found':
      return message.includes('not found') || message.includes('404');
    case 'already-exists':
      return message.includes('already exists') || message.includes('conflict');
    default:
      return false;
  }
}

/**
 * Get user-friendly error message for store operations
 */
export function getStoreErrorMessage(error: any, operation: 'create' | 'update' | 'delete' | 'fetch'): string {
  if (isStoreError(error, 'slug-taken')) {
    return 'This store URL is already taken. Please choose a different one.';
  }

  if (isStoreError(error, 'not-found')) {
    return 'Store not found. It may have been deleted or the URL is incorrect.';
  }

  if (isStoreError(error, 'already-exists')) {
    return 'You already have a store. You can only have one store per account.';
  }

  // Default messages based on operation
  switch (operation) {
    case 'create':
      return 'Failed to create store. Please check your information and try again.';
    case 'update':
      return 'Failed to update store. Please try again.';
    case 'delete':
      return 'Failed to delete store. Please try again.';
    case 'fetch':
      return 'Failed to load store information. Please try again.';
    default:
      return 'An unexpected error occurred. Please try again.';
  }
}
