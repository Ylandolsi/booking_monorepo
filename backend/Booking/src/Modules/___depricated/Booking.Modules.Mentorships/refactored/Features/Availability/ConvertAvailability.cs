using Booking.Modules.Mentorships.refactored.Features.Utils;

namespace Booking.Modules.Mentorships.refactored.Features.Availability;

public static class ConvertAvailability
{
    public static (DateTime, DateTime ) Convert(
        TimeOnly StartTime,
        TimeOnly EndTime,
        DateOnly Date,
        string TimeZoneIdFrom,
        string TimeZoneIdTo)
    {
        var convertedToUTCStart =
            TimeConvertion.ToInstant(Date, StartTime, TimeZoneIdFrom);

        var convertedToUTCEnd =
            TimeConvertion.ToInstant(Date, EndTime, TimeZoneIdFrom);

        var convertedToMenteeTimeZoneStart =
            TimeConvertion.ConvertInstantToTimeZone(convertedToUTCStart, TimeZoneIdTo);

        var convertedToMenteeTimeZoneEnd =
            TimeConvertion.ConvertInstantToTimeZone(convertedToUTCEnd, TimeZoneIdTo);

        return (convertedToMenteeTimeZoneStart, convertedToMenteeTimeZoneEnd);
    }
}