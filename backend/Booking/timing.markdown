# Handling Date and Time in .NET and Frontend Applications
https://learn.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime


This guide explains how to handle date and time in .NET applications and their interaction with frontend systems, covering `TimeSpan`, `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, and the `NodaTime` library for robust timezone handling.

## Core .NET Date and Time Types

### TimeSpan
- **Definition**: Represents a duration of time (the difference between two points in time).
- **Key Features**:
    - Does not include date information, only hours, minutes, seconds, and milliseconds.
    - Can be negative if the start time is later than the end time.
- **Examples**:
    - `TimeSpan.FromMinutes(90)` → Represents 1 hour and 30 minutes.
    - Subtracting two `DateTime` objects: `endTime - startTime` → Returns a `TimeSpan`.

### DateTime
- **Definition**: Represents a specific date and time with an optional `Kind` property (`Local`, `Utc`, or `Unspecified`).
- **Key Features**:
    - Used for specific points in time, such as August 21, 2025, 22:15:00.
    - ISO-8601 compliant for standard formatting.
- **Examples**:
    - `new DateTime(2025, 8, 21, 22, 15, 0)` → August 21, 2025, 10:15:00 PM.
    - Use `date.ToUTCString()` to convert to UTC or `date.ToLocalString()` for the client’s local timezone.

### DateTimeOffset
- **Definition**: Represents a specific date and time with an associated UTC offset, ensuring unambiguous time representation.
- **Key Features**:
    - Stores both the time and its offset from UTC (e.g., `+01:00` for Paris in winter).
    - Useful for preserving timezone information across systems.
- **Examples**:
    - `DateTimeOffset.UtcNow` → Returns the current UTC time.
    - `new DateTimeOffset(2025, 8, 21, 22, 15, 0, TimeSpan.FromHours(1))` → August 21, 2025, 10:15:00 PM with UTC+1 offset.

### DateOnly
- **Definition**: Represents a date without a time component.
- **Examples**:
    - `DateOnly.FromDateTime(DateTime.Now)` → Extracts the current date.
    - `new DateOnly(2025, 8, 21)` → August 21, 2025.

### TimeOnly
- **Definition**: Represents a time of day without a date component.
- **Examples**:
    - `new TimeOnly(9, 30)` → 09:30 AM.
    - `TimeOnly.FromDateTime(DateTime.Now)` → Extracts the current time.

## Timezone Handling with NodaTime
The `NodaTime` library is recommended for robust timezone handling, especially for recurring events and Daylight Saving Time (DST) transitions.

### Why Use NodaTime?
- Correctly handles DST transitions.
- Uses IANA timezone IDs (e.g., `Europe/Paris`), aligning with frontend systems.
- Ideal for recurring events and cross-timezone applications.

### Example: Converting UTC to Local Time
```csharp
using NodaTime;
using NodaTime.TimeZones;

var instant = Instant.FromDateTimeUtc(event.StartTimeUtc);
var tz = DateTimeZoneProviders.Tzdb[event.TimeZoneId]; // e.g., "Europe/Paris"
var localTime = instant.InZone(tz).ToDateTimeUnspecified();
```

### Handling Recurring Events
The problem: DST shifts → "9:00 AM New York" sometimes means 14:00 UTC (winter) and sometimes 13:00 UTC (summer).
```csharp
var tz = DateTimeZoneProviders.Tzdb["America/New_York"];
var localTime = new LocalTime(9, 0); // 9:00 AM
var startDate = new LocalDate(2025, 3, 1);
var recurrenceRule = "FREQ=WEEKLY;BYDAY=MO";

// Generate occurrences
var localStart = startDate.At(localTime).InZoneLeniently(tz);
var nextOccurrence = localStart.PlusWeeks(1); // still respects DST shift

```
backend save for exp : 9:00 AM  , timeZone : America/New_York
: 
saving utc here will cause some issues ! 

---

## Frontend and Backend Integration

### Frontend to Backend
- **Input**: Frontend sends a local datetime and an IANA timezone ID (e.g., `Europe/Paris`).
    - Example (JavaScript):
      ```javascript
      const timeZoneId = Intl.DateTimeFormat().resolvedOptions().timeZone;
      console.log(timeZoneId); // e.g., "Europe/Paris"
      ```
- **Processing**: Backend uses `NodaTime` to convert the local datetime to a `ZonedDateTime`, then to a `DateTimeOffset` with UTC and offset.

### Backend Storage
- **Storage**: Store `DateTimeOffset` in the database to capture the UTC instant and original offset.
- **Example Model**:
  ```csharp
  public class Event
  {
      public DateTimeOffset StartTimeUtc { get; set; } // Absolute instant
      public string TimeZoneId { get; set; }          // e.g., "Europe/Paris"
  }
  ```

### Backend to Frontend
- **Output**: Backend reads `DateTimeOffset` from the database, converts it to a `ZonedDateTime` using the original or requested timezone, and sends it as an ISO string (with offset or UTC).
- **Example**: `2025-08-21T22:15:00+01:00` for Paris time.

## Best Practices
- Use `DateTimeOffset` for storing absolute timestamps with offset information.
- Use `NodaTime` for complex timezone calculations and recurring events.
- Always store IANA timezone IDs alongside timestamps to preserve context.
- Avoid `DateTime` for cross-timezone applications unless `Kind` is explicitly set to `Utc`.


https://codeblog.jonskeet.uk/category/nodatime/
https://codeblog.jonskeet.uk/2019/03/27/storing-utc-is-not-a-silver-bullet/