interface UpdateStorePictureInput {
  file: File;
}

interface UpdateStorePictureResponse {
  slug: string;
}

export const updateStorePicture = async (data: UpdateStorePictureInput): Promise<UpdateStorePictureResponse> => {
  console.log('mock: updateStore with data:', data);
  return { slug: 'slug-123' };
};
