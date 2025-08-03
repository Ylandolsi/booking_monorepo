namespace Booking.Modules.Users.Features.Experience.Get;

public sealed record GetExperienceResponse
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Company { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Description { get; init; }
    public bool ToPresent { get; init; }
}
