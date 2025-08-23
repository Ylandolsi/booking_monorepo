using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.Entities.Days;

public static class DayErrors
{
    public static readonly Error AlreadyActive = Error.Problem("Day.AlreadyActive", "Day is already active");
    public static readonly Error AlreadyInactive = Error.Problem("Day.AlreadyInactive", "Day is already inactive");
}