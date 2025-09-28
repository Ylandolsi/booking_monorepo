using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.Entities.MentorshipRelationships;

public static class MentorshipRelationshipErrors
{
    public static readonly Error InactiveRelationship = Error.Problem(
        "MentorshipRelationship.InactiveRelationship",
        "Cannot add session to inactive mentorship relationship");

    public static readonly Error SessionMismatch = Error.Problem(
        "MentorshipRelationship.SessionMismatch",
        "Session mentor or mentee does not match relationship");

    public static readonly Error AlreadyInactive = Error.Problem(
        "MentorshipRelationship.AlreadyInactive",
        "Mentorship relationship is already inactive");

    public static readonly Error AlreadyActive = Error.Problem(
        "MentorshipRelationship.AlreadyActive",
        "Mentorship relationship is already active");

    public static readonly Error NotFound = Error.NotFound(
        "MentorshipRelationship.NotFound",
        "Mentorship relationship not found");

    public static Error NotFoundById(int id) => Error.NotFound(
        "MentorshipRelationship.NotFoundById",
        $"Mentorship relationship with ID {id} not found");
}