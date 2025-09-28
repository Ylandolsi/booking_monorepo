namespace Booking.Modules.Mentorships.refactored.Domain.Entities.Mentors;


public static class MentorErrors
{
    public static readonly Error InvalidHourlyRate = Error.Problem(
        "Mentor.InvalidHourlyRate",
        "Hourly rate must be greater than zero");

    public static readonly Error InvalidBufferTime = Error.Problem(
        "Mentor.InvalidBufferTime",
        "Buffer time must be in 15-minute increments and between 0 and 480 minutes");

    public static readonly Error AlreadyActive = Error.Problem(
        "Mentor.AlreadyActive",
        "Mentor is already active");

    public static readonly Error AlreadyInactive = Error.Problem(
        "Mentor.AlreadyInactive",
        "Mentor is already inactive");

    public static readonly Error NotFound = Error.NotFound(
        "Mentor.NotFound",
        "Mentor not found");

    public static readonly Error AlreadyMentor = Error.Problem(
        "Mentor.AlreadyMentor",
        "User is already a mentor");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Mentor.NotFoundById",
        $"Mentor with ID {id} not found");

    public static Error NotFoundByUserId(int userId) => Error.NotFound(
        "Mentor.NotFoundByUserId",
        $"Mentor with User ID {userId} not found");
}