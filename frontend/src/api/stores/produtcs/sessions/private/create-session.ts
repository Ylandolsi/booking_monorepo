import { patchPostSessionSchemaToFormData } from '@/api/stores/produtcs';
import type { CreateSessionProductRequest, PatchPostSessionResponse } from '@/api/stores/produtcs/sessions/private/schema-session';
import { api, CatalogEndpoints, validateFile } from '@/lib/api';

export const updateSession = async (data: CreateSessionProductRequest, productSlug: string) => {
  if (data.thumbnail) {
    const validation = validateFile(data.thumbnail);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostSessionSchemaToFormData(data);

  try {
    const response = await api.put<PatchPostSessionResponse>(CatalogEndpoints.Products.Sessions.Update(productSlug), formData);

    return response;
  } catch (error) {
    console.error('Error updating session:', error);
    throw error;
  }
};
