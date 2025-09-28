/* eslint-disable @typescript-eslint/no-explicit-any */
import { env } from '@/config/env';
import { RefreshAccessToken } from '@/api/utils/auth-endpoints';
import { toast } from 'sonner';

export type RequestOptions = {
  method?: string;
  headers?: Record<string, string>;
  body?: any;
  params?: Record<string, string | number | boolean | undefined | null>;
  cache?: RequestCache;
};

export function buildUrlWithParams(url: string, params?: RequestOptions['params']): string {
  if (!params) return url;
  const filteredParams = Object.fromEntries(Object.entries(params).filter(([, value]) => value !== undefined && value !== null));
  if (Object.keys(filteredParams).length === 0) return url;
  const queryString = new URLSearchParams(filteredParams as Record<string, string>).toString();
  return `${url}?${queryString}`;
}

async function fetchApi<T>(url: string, options: RequestOptions = {}): Promise<T> {
  const { method = 'GET', headers = {}, body, params, cache = 'no-store' } = options;

  const fullUrl = buildUrlWithParams(`${env.API_URL}/${url}`, params);

  // Handle FormData bodies (e.g., file uploads) by letting the browser set multipart headers
  const isFormData = body instanceof FormData;
  const fetchHeaders: Record<string, string> = {
    Accept: 'application/json',
    // Only set JSON content type for non-FormData bodies
    ...(isFormData ? {} : { 'Content-Type': 'application/json' }),
    ...headers,
  };
  const requestBody = isFormData ? body : body ? JSON.stringify(body) : undefined;
  const response = await fetch(fullUrl, {
    method,
    headers: fetchHeaders,
    body: requestBody,
    credentials: 'include',
    cache,
  });

  // handle unauthorized access
  if (response.status === 401) {
    if (window.location.href.includes('auth')) return undefined as T; // Allow auth pages to handle 401 without redirecting
    // try to refresh the token
    try {
      const refreshResponse = await fetch(`${env.API_URL}/${RefreshAccessToken}`, {
        method: 'POST',
        credentials: 'include',
      });

      if (refreshResponse.ok) {
        // If token refresh is successful, retry the original request
        return fetchApi<T>(url, options);
      }
    } catch (error) {
      console.error('Token refresh failed:', error);
    }
    // If the token refresh fails, redirect to login
    window.location.href = '/auth/login?redirectTo=' + encodeURIComponent(window.location.pathname + window.location.search);
    toast.error('Unauthorized access');
  }

  // Handle 204 No Content
  if (response.status === 204) {
    return undefined as T;
  }

  // Handle 201 Created with or without body
  if (response.status === 201) {
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return response.json();
    }
    return undefined as T;
  }

  if (!response.ok) {
    let message = response.statusText;
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('json')) {
      try {
        const data: ErrorType = await response.json();
        message = data.detail || data.title || message;
        // TODO :
        // access data.error for validation errors
        // Example: if (data.error) { ... }
        //         "errors": [
        //     {
        //         "code": "NotEmptyValidator",
        //         "description": "At least one day availability must be provided.",
        //         "type": 2
        //     },
        //     {
        //         "code": "PredicateValidator",
        //         "description": "Invalid availability configuration. Time ranges must be in HH:mm format and end time must be after start time.",
        //         "type": 2
        //     }
        // ],
        if (data.error) {
          console.error('Validation errors:', data.error);
          data.error.forEach((err) => {
            message += `\n- ${err.description}`;
          });
        }
      } catch {
        /* ignore */
      }
    } else {
      try {
        const text = await response.text();
        if (text) message = text;
      } catch {
        /* ignore */
      }
    }

    // Show toast error notification
    toast.error(message);
    // await Promise.reject(new Error(message));
    throw new Error(message);
  }

  const contentType = response.headers.get('content-type');
  if (contentType && contentType.includes('application/json')) {
    try {
      return await response.json();
    } catch {
      return undefined as T;
    }
  }
  return undefined as T;
}

export const api = {
  get<T>(url: string, options?: RequestOptions): Promise<T> {
    return fetchApi<T>(url, { ...options, method: 'GET' });
  },
  post<T>(url: string, body?: any, options?: RequestOptions): Promise<T> {
    return fetchApi<T>(url, { ...options, method: 'POST', body });
  },
  put<T>(url: string, body?: any, options?: RequestOptions): Promise<T> {
    return fetchApi<T>(url, { ...options, method: 'PUT', body });
  },
  patch<T>(url: string, body?: any, options?: RequestOptions): Promise<T> {
    return fetchApi<T>(url, { ...options, method: 'PATCH', body });
  },
  delete<T>(url: string, options?: RequestOptions): Promise<T> {
    return fetchApi<T>(url, { ...options, method: 'DELETE' });
  },
};

type ValidationError = {
  code: string;
  description: string;
  type: number;
};

type ErrorType = {
  type: string;
  title: string;
  status: number;
  detail: string;
  error: ValidationError[] | null;
};
