namespace Booking.Modules.Mentorships.refactored.Features.Utils;

public static class TimeConvertion
{
    public static DateTime ConvertInstantToTimeZone(DateTime utcInstant, string targetTimeZoneId)
    {
        var targetTz = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(utcInstant, targetTz);
    }
    
    public static DateTime ToInstant(DateOnly date, TimeOnly time, string timeZoneId)
    {
        // 1. Combine DateOnly + TimeOnly into a DateTime (local "unspecified" kind)
        var localDateTime = date.ToDateTime(time, DateTimeKind.Unspecified);

        // 2. Find the time zone
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        // 3. Convert local time in that zone to UTC
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, tz);

        return utcDateTime;

        /*
            var date = new DateOnly(2025, 8, 22);
            var time = new TimeOnly(14, 30); // 2:30 PM
            var timeZoneId = "Africa/Tunis"; // Linux/Mac (on Windows use "Romance Standard Time")

            var instant = ToInstant(date, time, timeZoneId);

            Console.WriteLine($"Local: {date} {time} ({timeZoneId})");
            Console.WriteLine($"UTC Instant: {instant:yyyy-MM-dd HH:mm:ss}Z");
        */
    }
}