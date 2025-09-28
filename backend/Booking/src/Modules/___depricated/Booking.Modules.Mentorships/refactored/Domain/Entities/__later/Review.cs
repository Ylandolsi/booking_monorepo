using System.ComponentModel.DataAnnotations.Schema;
using Booking.Modules.Mentorships.refactored.Domain.Entities.Sessions;

namespace Booking.Modules.Mentorships.refactored.Domain.Entities.__later;

public class Review : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public int SessionId { get; private set; }
    
    public int MentorId { get; private set; }
    
    public int MenteeId { get; private set; }
    
    public int Rating { get; private set; } // 1-5 stars
    
    public string Comment { get; private set; } = string.Empty;
    
    // Navigation properties
    public Session Session { get; set; } = default!;

    private Review() { }

    public static Review Create(int sessionId, int mentorId, int menteeId, int rating, string comment)
    {
        var review = new Review
        {
            SessionId = sessionId,
            MentorId = mentorId,
            MenteeId = menteeId,
            Rating = rating,
            Comment = comment?.Trim() ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        return review;
    }

    public Result UpdateReview(int rating, string? comment = null)
    {
        if (rating < 1 || rating > 5)
        {
            return Result.Failure(ReviewErrors.InvalidRating);
        }

        // Check if review is too old to update (e.g., 30 days)
        if (DateTime.UtcNow - CreatedAt > TimeSpan.FromDays(30))
        {
            return Result.Failure(ReviewErrors.ReviewTooOldToUpdate);
        }

        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}

public static class ReviewErrors
{
    public static readonly Error InvalidRating = Error.Problem(
        "Review.InvalidRating",
        "Rating must be between 1 and 5");

    public static readonly Error ReviewTooOldToUpdate = Error.Problem(
        "Review.ReviewTooOldToUpdate",
        "Review cannot be updated after 30 days");

    public static readonly Error NotFound = Error.NotFound(
        "Review.NotFound",
        "Review not found");

    public static readonly Error AlreadyExists = Error.Conflict(
        "Review.AlreadyExists",
        "User has already reviewed this session");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Review.NotFoundById",
        $"Review with ID {id} not found");
}
