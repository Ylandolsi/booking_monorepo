/**
 * Helper function to convert object to FormData
 * Handles File objects, arrays, and nested objects
 */
export function toFormData<T extends Record<string, any>>(obj: T): FormData {
  const formData = new FormData();

  Object.entries(obj).forEach(([key, value]) => {
    if (value === undefined || value === null) {
      return; // Skip undefined/null values
    }

    if (value instanceof File) {
      formData.append(key, value);
    } else if (Array.isArray(value)) {
      formData.append(key, JSON.stringify(value));
    } else if (typeof value === 'object') {
      formData.append(key, JSON.stringify(value));
    } else {
      formData.append(key, String(value));
    }
  });

  return formData;
}

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
