// ===== UPDATE STORE =====

// ===== UPDATE STORE PICTURE =====

// ===== CHECK SLUG AVAILABILITY =====

// ===== VALIDATION HELPERS =====

/**
 * Validate store creation input
 */
// export function validateCreateStoreInput(data: CreateStoreInput): { isValid: boolean; errors: string[] } {
//   const errors: string[] = [];

//   // Title validation
//   if (!data.title?.trim()) {
//     errors.push('Store title is required');
//   } else if (data.title.length > 100) {
//     errors.push('Store title cannot exceed 100 characters');
//   }

//   // Slug validation
//   if (!data.slug?.trim()) {
//     errors.push('Store slug is required');
//   } else if (data.slug.length > 50) {
//     errors.push('Store slug cannot exceed 50 characters');
//   } else if (!/^[a-z0-9-]+$/.test(data.slug)) {
//     errors.push('Store slug can only contain lowercase letters, numbers, and hyphens');
//   }

//   // Description validation
//   if (data.description && data.description.length > 1000) {
//     errors.push('Store description cannot exceed 1000 characters');
//   }

//   // Picture validation
//   if (data.picture) {
//     const fileValidation = validateFile(data.picture, {
//       maxSizeInMB: 5,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!fileValidation.isValid) {
//       errors.push(fileValidation.error || 'Invalid picture file');
//     }
//   }

//   // Social links validation
//   if (data.socialLinks) {
//     data.socialLinks.forEach((link, index) => {
//       if (!link.platform?.trim()) {
//         errors.push(`Social link ${index + 1}: Platform is required`);
//       }
//       if (!link.url?.trim()) {
//         errors.push(`Social link ${index + 1}: URL is required`);
//       } else {
//         try {
//           new URL(link.url);
//         } catch {
//           errors.push(`Social link ${index + 1}: Invalid URL format`);
//         }
//       }
//     });
//   }

//   return {
//     isValid: errors.length === 0,
//     errors,
//   };
// }

// export function validateUpdateStoreInput(data: UpdateStoreInput): { isValid: boolean; errors: string[] } {
//   // Reuse most validations from create, but picture is not included in update
//   const createData: CreateStoreInput = {
//     title: data.title,
//     slug: data.slug,
//     description: data.description,
//     socialLinks: data.socialLinks,
//     // No picture in update
//   };

//   return validateCreateStoreInput(createData);
// }

// ===== ERROR HANDLING HELPERS =====

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
