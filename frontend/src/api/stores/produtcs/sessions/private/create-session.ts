import { patchPostSessionSchemaToFormData } from '@/api/stores/produtcs';
import type { CreateSessionProductRequest, PatchPostSessionResponse } from '@/api/stores/produtcs/sessions/private/schema-session';
import { api, CatalogEndpoints, validateFile } from '@/lib/api';
import { useMutation } from '@tanstack/react-query';

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
    const response = await api.post<PatchPostSessionResponse>(CatalogEndpoints.Products.Sessions.Create, formData);

    return response;
  } catch (error) {
    console.error('Error creating session:', error);
    throw error;
  }
};

export const useCreateSession = () => {
  return useMutation({
    mutationFn: (data: CreateSessionProductRequest) => createSession(data),
    meta: {
      // invalidatesQuery: [SESSION_QUERY_KEY],
      successMessage: 'Session created successfully!',
    },
  });
};
