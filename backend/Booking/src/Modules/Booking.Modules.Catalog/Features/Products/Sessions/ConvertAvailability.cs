using Booking.Modules.Catalog.Features.Utils;

namespace Booking.Modules.Catalog.Features.Products.Sessions;

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