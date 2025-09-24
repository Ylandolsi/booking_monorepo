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
