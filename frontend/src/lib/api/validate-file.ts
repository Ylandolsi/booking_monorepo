/**
 * Helper function to validate file types and sizes
 */
export interface FileValidationOptions {
  maxSizeInMB?: number;
  allowedTypes?: string[];
  required?: boolean;
}

export function validateFile(file: File | undefined, options: FileValidationOptions = {}): { isValid: boolean; error?: string } {
  const { maxSizeInMB = 5, allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'], required = false } = options;

  if (!file) {
    return { isValid: !required, error: required ? 'File is required' : undefined };
  }

  if (!allowedTypes.includes(file.type)) {
    return {
      isValid: false,
      error: `File type not allowed. Allowed types: ${allowedTypes.join(', ')}`,
    };
  }

  const maxSizeInBytes = maxSizeInMB * 1024 * 1024;
  if (file.size > maxSizeInBytes) {
    return {
      isValid: false,
      error: `File size exceeds ${maxSizeInMB}MB limit`,
    };
  }

  return { isValid: true };
}
