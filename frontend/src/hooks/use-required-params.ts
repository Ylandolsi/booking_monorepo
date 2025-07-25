import { useParams } from '@tanstack/react-router';

export function useRequiredParam(paramName: string): string | null {
  const params = useParams({ strict: false }) as Record<
    string,
    string | undefined
  >;
  const value = params[paramName];
  return value ?? null;
}

export function useRequiredParams(
  paramNames: string[],
): Record<string, string> | null {
  const params = useParams({ strict: false }) as Record<
    string,
    string | undefined
  >;
  const result: Record<string, string> = {};
  for (const paramName of paramNames) {
    const value = params[paramName];
    if (!value) return null;
    result[paramName] = value;
  }
  return result;
}
