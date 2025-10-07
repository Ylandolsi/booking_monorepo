using Booking.Common.Results;

namespace Booking.Common;

public static class DatabaseErrors
{
    public static Error SaveChangeError(string details)
    {
        return Error.Failure("Database.SaveChanges", details);
    }
}