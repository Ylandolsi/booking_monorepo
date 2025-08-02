namespace Booking.Modules.Users.Features.Experience.Get;

public sealed record GetExperienceResponse(
    int Id,
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    bool ToPresent
);



