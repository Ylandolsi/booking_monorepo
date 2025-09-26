/**
 * Comprehensive Error Handling and Validation for Catalog API
 * Handles edge cases, network failures, file constraints, and user-friendly error messages
 */

import { toast } from 'sonner';

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

  return { handleError };
}

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
