import { patchPostSessionSchemaToFormData } from '@/api/stores/produtcs';
import type {
  CreateProductInput,
  CreateSessionProductRequest,
  PatchPostSessionResponse,
} from '@/api/stores/produtcs/sessions/private/schema-session';
import { api, CatalogEndpoints, validateFile } from '@/lib/api';
import { useMutation } from '@tanstack/react-query';

export const updateSession = async ({ data, productSlug }: { data: CreateProductInput; productSlug: string }): Promise<PatchPostSessionResponse> => {
  if (data.thumbnailImage) {
    const validation = validateFile(data.thumbnailImage);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostSessionSchemaToFormData(data as CreateSessionProductRequest);

  try {
    const response = await api.put<PatchPostSessionResponse>(CatalogEndpoints.Products.Sessions.Update(productSlug), formData);

    return response;
  } catch (error) {
    console.error('Error updating session:', error);
    throw error;
  }
};

export const useUpdateSession = () => {
  return useMutation<PatchPostSessionResponse, Error, { data: CreateProductInput; productSlug: string }>({
    mutationFn: updateSession,
    meta: {
      // invalidatesQuery: [SESSION_QUERY_KEY],
      successMessage: 'Session updated successfully!',
    },
  });
};
