export function GenerateTimeZoneId() {
  return Intl.DateTimeFormat().resolvedOptions().timeZone;
}
export function ConvertUtcToLocal(timeZoneId?: string) {
  const localTime = new Date().toLocaleString('en-US', {
    timeZone: timeZoneId ?? GenerateTimeZoneId(),
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    timeZoneName: 'short',
  });

  return localTime;
}
// "en-US" = U.S. English formatting.

// Example: August 25, 2025, 08:45:12 PM GMT+2

// "en-GB" = U.K. English formatting.

// Example: 25 August 2025 at 20:45:12 GMT+2

// "fr-FR" = French formatting.

// Example: 25 août 2025 à 20:45:12 UTC+2
