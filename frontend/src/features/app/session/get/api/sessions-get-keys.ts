export const sessionQueryKeys = {
  session: (upToDate?: string, timeZoneId?: string) =>
    ['sessions', upToDate, timeZoneId] as const,
};
