import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/api/utils/catalog-endpoints';
import { api, buildUrlWithParams } from '@/api/utils';
import { logger } from '@/lib';

export const getAllSessionMonthly = async ({
  year,
  month,
  timeZoneId,
}: {
  year: number;
  month: number;
  timeZoneId: string;
}): Promise<MonthlySessionsResponse> => {
  try {
    // Assuming the endpoint is updated to accept year, month, timeZoneId
    const queryParams = { year, month, timeZoneId };
    const urlEndpoint = buildUrlWithParams(CatalogEndpoints.Products.Sessions.GetAllSessionsMonthly(), queryParams);

    const response = await api.get<MonthlySessionsResponse>(urlEndpoint);

    return response;
  } catch (error) {
    logger.error('Error fetching sessions:', error);
    throw error;
  }
};

export function useAllSessionMonthly(
  { year, month, timeZoneId }: { year: number; month: number; timeZoneId: string },
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<MonthlySessionsResponse, Error> {
  return useQuery(
    queryOptions({
      // TODO : add product slug to the key
      queryKey: ['all-sessions', { year, month, timeZoneId }],
      queryFn: () => getAllSessionMonthly({ year, month, timeZoneId }),
      ...overrides,
    }),
  );
}

export interface MonthlySessionsResponse {
  year: number;
  month: number; // 1-12
  days: DailySessions[];
}

export interface DailySessions {
  day: number; // 1-31
  active: string; // "active" or "inactive" for frontend styling
  sessions: SessionResponse[];
}

export interface SessionResponse {
  id: number;
  title: string;
  location: string; // e.g., "Google Meet"
  scheduledAt: Date; // In user's timezone
  scheduledTimeZone: string; // e.g., "America/New_York"
  date: string; // e.g., 'Thu, 30 Nov'
  time: string; // e.g., '11:00 AM - 12:00 PM'
  participants: string[];
  notes: string;
  status: SessionStatus;
  googleMeetLink?: string;
  durationInMinutes: number;
  price: number;
  createdAt: Date;
  updatedAt: Date;
  completedAt?: Date;
}

type SessionStatus = 'Booked' | 'WaitingForPayment' | 'Confirmed' | 'Completed' | 'Cancelled' | 'NoShow';
