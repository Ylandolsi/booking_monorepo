import { patchPostSessionSchemaToFormData } from '@/api/stores/produtcs';
import type { CreateSessionProductRequest, PatchPostSessionResponse } from '@/api/stores/produtcs/sessions/private/schema-session';
import { api, CatalogEndpoints, validateFile } from '@/lib/api';

export const createSession = async (data: CreateSessionProductRequest) => {
  if (data.thumbnail) {
    const validation = validateFile(data.thumbnail);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostSessionSchemaToFormData(data);

  try {
    const response = await api.put<PatchPostSessionResponse>(CatalogEndpoints.Products.Sessions.Update, formData);

    return response;
  } catch (error) {
    console.error('Error creating session:', error);
    throw error;
  }
};
