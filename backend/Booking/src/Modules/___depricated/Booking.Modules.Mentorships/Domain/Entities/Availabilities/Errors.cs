using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.Entities.Availabilities;

public static class AvailabilityErrors
{
    public static readonly Error AlreadyActive = Error.Problem(
        "Availability.AlreadyActive",
        "Availability is already active");

    public static readonly Error AlreadyInactive = Error.Problem(
        "Availability.AlreadyInactive",
        "Availability is already inactive");

    public static readonly Error NotFound = Error.NotFound(
        "Availability.NotFound",
        "Availability not found");

    public static readonly Error ConflictingTimeRange = Error.Conflict(
        "Availability.ConflictingTimeRange",
        "Availability time range conflicts with existing availability");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Availability.NotFoundById",
        $"Availability with ID {id} not found");
}