/**
 * Comprehensive Error Handling and Validation for Catalog API
 * Handles edge cases, network failures, file constraints, and user-friendly error messages
 */

import { toast } from 'sonner';
import type { ApiError, ValidationError } from '@/types/catalog-api';

// ===== API ERROR TYPES =====

export interface CatalogApiError extends Error {
  status?: number;
  code?: string;
  type?: string;
  details?: ValidationError[];
}

export class CatalogError extends Error implements CatalogApiError {
  public status?: number;
  public code?: string;
  public type?: string;
  public details?: ValidationError[];

  constructor(message: string, status?: number, code?: string, type?: string, details?: ValidationError[]) {
    super(message);
    this.name = 'CatalogError';
    this.status = status;
    this.code = code;
    this.type = type;
    this.details = details;
  }

  static fromApiError(apiError: ApiError): CatalogError {
    return new CatalogError(apiError.detail || apiError.title, apiError.status, apiError.type, apiError.title, apiError.errors);
  }
}

// ===== ERROR CLASSIFICATION =====

export function classifyError(error: any): {
  type: 'network' | 'validation' | 'authorization' | 'conflict' | 'server' | 'unknown';
  isRetryable: boolean;
  userMessage: string;
  technicalDetails?: string;
} {
  // Network errors
  if (error instanceof TypeError && error.message.includes('fetch')) {
    return {
      type: 'network',
      isRetryable: true,
      userMessage: 'Network connection failed. Please check your internet connection and try again.',
      technicalDetails: error.message,
    };
  }

  // API errors with status codes
  if (error.status || error.response?.status) {
    const status = error.status || error.response?.status;

    switch (status) {
      case 400:
        return {
          type: 'validation',
          isRetryable: false,
          userMessage: 'Invalid request. Please check your input and try again.',
          technicalDetails: error.message,
        };

      case 401:
        return {
          type: 'authorization',
          isRetryable: false,
          userMessage: 'You need to log in to perform this action.',
          technicalDetails: 'Unauthorized access',
        };

      case 403:
        return {
          type: 'authorization',
          isRetryable: false,
          userMessage: "You don't have permission to perform this action.",
          technicalDetails: 'Forbidden access',
        };

      case 404:
        return {
          type: 'validation',
          isRetryable: false,
          userMessage: 'The requested resource was not found.',
          technicalDetails: error.message,
        };

      case 409:
        return {
          type: 'conflict',
          isRetryable: false,
          userMessage: 'There was a conflict with your request. The resource may already exist.',
          technicalDetails: error.message,
        };

      case 413:
        return {
          type: 'validation',
          isRetryable: false,
          userMessage: "The file you're trying to upload is too large.",
          technicalDetails: 'Payload too large',
        };

      case 422:
        return {
          type: 'validation',
          isRetryable: false,
          userMessage: 'The data you provided is invalid. Please check your input.',
          technicalDetails: error.message,
        };

      case 429:
        return {
          type: 'server',
          isRetryable: true,
          userMessage: 'Too many requests. Please wait a moment and try again.',
          technicalDetails: 'Rate limit exceeded',
        };

      case 500:
      case 502:
      case 503:
      case 504:
        return {
          type: 'server',
          isRetryable: true,
          userMessage: 'Server error. Please try again in a few moments.',
          technicalDetails: `Server error ${status}`,
        };

      default:
        return {
          type: 'unknown',
          isRetryable: false,
          userMessage: 'An unexpected error occurred. Please try again.',
          technicalDetails: error.message,
        };
    }
  }

  // Validation errors from our own validation functions
  if (error.message && typeof error.message === 'string') {
    if (error.message.includes('required')) {
      return {
        type: 'validation',
        isRetryable: false,
        userMessage: error.message,
        technicalDetails: 'Client-side validation error',
      };
    }

    if (error.message.includes('file') || error.message.includes('size') || error.message.includes('type')) {
      return {
        type: 'validation',
        isRetryable: false,
        userMessage: error.message,
        technicalDetails: 'File validation error',
      };
    }
  }

  // Default unknown error
  return {
    type: 'unknown',
    isRetryable: false,
    userMessage: 'An unexpected error occurred. Please try again.',
    technicalDetails: error.message || 'Unknown error',
  };
}

// ===== ERROR HANDLING HOOKS =====

export function useErrorHandler() {
  const handleError = (error: any, context?: string) => {
    const classified = classifyError(error);

    // Log technical details for debugging
    console.error(`[${context || 'API Error'}]`, {
      type: classified.type,
      userMessage: classified.userMessage,
      technicalDetails: classified.technicalDetails,
      originalError: error,
    });

    // Show user-friendly toast
    toast.error(classified.userMessage);

    return classified;
  };

  const handleErrorWithRetry = (error: any, retryFn?: () => void, context?: string) => {
    const classified = handleError(error, context);

    if (classified.isRetryable && retryFn) {
      toast.error(classified.userMessage, {
        action: {
          label: 'Retry',
          onClick: retryFn,
        },
      });
    }

    return classified;
  };

  return { handleError, handleErrorWithRetry };
}

// ===== VALIDATION UTILITIES =====

export const FileValidation: {
  images: string[];
  documents: string[];
  maxImageSize: number;
  maxDocumentSize: number;
  maxProfilePictureSize: number;
  validateFileType: (file: File, allowedTypes: string[]) => { valid: boolean; error?: string };
  validateFileSize: (file: File, maxSizeMB: number) => { valid: boolean; error?: string };
  validateImage: (file: File, maxSizeMB?: number) => { valid: boolean; errors: string[] };
} = {
  // Common file type groups
  images: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
  documents: ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'],

  // Size limits in MB
  maxImageSize: 10,
  maxDocumentSize: 25,
  maxProfilePictureSize: 5,

  validateFileType(file: File, allowedTypes: string[]): { valid: boolean; error?: string } {
    if (!allowedTypes.includes(file.type)) {
      return {
        valid: false,
        error: `File type '${file.type}' is not allowed. Allowed types: ${allowedTypes.join(', ')}`,
      };
    }
    return { valid: true };
  },

  validateFileSize(file: File, maxSizeMB: number): { valid: boolean; error?: string } {
    const maxSizeBytes = maxSizeMB * 1024 * 1024;
    if (file.size > maxSizeBytes) {
      return {
        valid: false,
        error: `File size (${(file.size / 1024 / 1024).toFixed(1)}MB) exceeds limit of ${maxSizeMB}MB`,
      };
    }
    return { valid: true };
  },

  validateImage(file: File, maxSizeMB: number = FileValidation.maxImageSize): { valid: boolean; errors: string[] } {
    const errors: string[] = [];

    const typeCheck = FileValidation.validateFileType(file, FileValidation.images);
    if (!typeCheck.valid) errors.push(typeCheck.error!);

    const sizeCheck = FileValidation.validateFileSize(file, maxSizeMB);
    if (!sizeCheck.valid) errors.push(sizeCheck.error!);

    return { valid: errors.length === 0, errors };
  },
};

export const TextValidation = {
  slug: {
    minLength: 3,
    maxLength: 50,
    pattern: /^[a-z0-9-]+$/,
    validate(slug: string): { valid: boolean; errors: string[] } {
      const errors: string[] = [];

      if (!slug || slug.length < TextValidation.slug.minLength) {
        errors.push(`Slug must be at least ${TextValidation.slug.minLength} characters long`);
      }

      if (slug.length > TextValidation.slug.maxLength) {
        errors.push(`Slug cannot exceed ${TextValidation.slug.maxLength} characters`);
      }

      if (!TextValidation.slug.pattern.test(slug)) {
        errors.push('Slug can only contain lowercase letters, numbers, and hyphens');
      }

      if (slug.startsWith('-') || slug.endsWith('-')) {
        errors.push('Slug cannot start or end with a hyphen');
      }

      if (slug.includes('--')) {
        errors.push('Slug cannot contain consecutive hyphens');
      }

      return { valid: errors.length === 0, errors };
    },
  },

  title: {
    minLength: 1,
    maxLength: 100,
    validate(title: string): { valid: boolean; errors: string[] } {
      const errors: string[] = [];

      if (!title || title.trim().length < TextValidation.title.minLength) {
        errors.push('Title is required');
      }

      if (title.length > TextValidation.title.maxLength) {
        errors.push(`Title cannot exceed ${TextValidation.title.maxLength} characters`);
      }

      return { valid: errors.length === 0, errors };
    },
  },

  description: {
    maxLength: 1000,
    validate(description?: string): { valid: boolean; errors: string[] } {
      const errors: string[] = [];

      if (description && description.length > TextValidation.description.maxLength) {
        errors.push(`Description cannot exceed ${TextValidation.description.maxLength} characters`);
      }

      return { valid: errors.length === 0, errors };
    },
  },

  email: {
    pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    validate(email: string): { valid: boolean; errors: string[] } {
      const errors: string[] = [];

      if (!email || !email.trim()) {
        errors.push('Email is required');
      } else if (!TextValidation.email.pattern.test(email)) {
        errors.push('Invalid email format');
      }

      return { valid: errors.length === 0, errors };
    },
  },

  url: {
    validate(url: string): { valid: boolean; errors: string[] } {
      const errors: string[] = [];

      if (!url || !url.trim()) {
        errors.push('URL is required');
      } else {
        try {
          new URL(url);
        } catch {
          errors.push('Invalid URL format');
        }
      }

      return { valid: errors.length === 0, errors };
    },
  },
};

export const PriceValidation = {
  min: 0,
  max: 10000,
  validate(price: number): { valid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (typeof price !== 'number' || isNaN(price)) {
      errors.push('Price must be a valid number');
    } else {
      if (price < PriceValidation.min) {
        errors.push(`Price cannot be less than ${PriceValidation.min}`);
      }

      if (price > PriceValidation.max) {
        errors.push(`Price cannot exceed ${PriceValidation.max}`);
      }
    }

    return { valid: errors.length === 0, errors };
  },
};

export const TimeValidation = {
  validateTimeRange(startTime: string, endTime: string): { valid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (!startTime || !endTime) {
      errors.push('Both start and end times are required');
      return { valid: false, errors };
    }

    try {
      const start = new Date(`2000-01-01 ${startTime}`);
      const end = new Date(`2000-01-01 ${endTime}`);

      if (isNaN(start.getTime()) || isNaN(end.getTime())) {
        errors.push('Invalid time format');
      } else if (start >= end) {
        errors.push('End time must be after start time');
      }
    } catch {
      errors.push('Invalid time format');
    }

    return { valid: errors.length === 0, errors };
  },

  validateFutureDateTime(dateTime: string): { valid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (!dateTime) {
      errors.push('Date and time are required');
      return { valid: false, errors };
    }

    try {
      const date = new Date(dateTime);
      const now = new Date();

      if (isNaN(date.getTime())) {
        errors.push('Invalid date format');
      } else if (date <= now) {
        errors.push('Date and time must be in the future');
      }
    } catch {
      errors.push('Invalid date format');
    }

    return { valid: errors.length === 0, errors };
  },
};

// ===== NETWORK UTILITIES =====

export const NetworkUtils = {
  async withRetry<T>(fn: () => Promise<T>, maxRetries: number = 3, delay: number = 1000): Promise<T> {
    let lastError: Error;

    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      try {
        return await fn();
      } catch (error) {
        lastError = error as Error;

        const classified = classifyError(error);

        // Don't retry non-retryable errors
        if (!classified.isRetryable) {
          throw error;
        }

        // Don't delay on the last attempt
        if (attempt < maxRetries) {
          await new Promise((resolve) => setTimeout(resolve, delay * attempt));
        }
      }
    }

    throw lastError!;
  },

  isOnline(): boolean {
    return navigator.onLine;
  },

  async waitForConnection(): Promise<void> {
    if (NetworkUtils.isOnline()) return;

    return new Promise((resolve) => {
      const handleOnline = () => {
        window.removeEventListener('online', handleOnline);
        resolve();
      };

      window.addEventListener('online', handleOnline);
    });
  },
};

// ===== FORM UTILITIES =====

export const FormUtils = {
  formatErrors(errors: string[]): string {
    if (errors.length === 0) return '';
    if (errors.length === 1) return errors[0];

    return errors.map((error, index) => `${index + 1}. ${error}`).join('\n');
  },

  validateForm<T extends Record<string, any>>(
    data: T,
    validators: { [K in keyof T]?: (value: T[K]) => { valid: boolean; errors: string[] } },
  ): { valid: boolean; errors: Record<keyof T, string[]>; allErrors: string[] } {
    const errors = {} as Record<keyof T, string[]>;
    const allErrors: string[] = [];

    for (const field in validators) {
      const validator = validators[field];
      if (validator) {
        const result = validator(data[field]);
        errors[field] = result.errors;
        allErrors.push(...result.errors);
      }
    }

    return {
      valid: allErrors.length === 0,
      errors,
      allErrors,
    };
  },
};
