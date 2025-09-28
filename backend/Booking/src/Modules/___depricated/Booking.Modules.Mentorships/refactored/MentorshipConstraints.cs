namespace Booking.Modules.Mentorships.refactored;

public static class MentorshipConstraints
{
    // Session constraints
    public const int MinSessionDurationMinutes = 30;
    public const int MaxSessionDurationMinutes = 60;
    
    // Availability constraints
    public const int MinWorkingHour = 0;
    public const int MaxWorkingHour = 23;
    
    // Review constraints
    public const int MinRating = 1;
    public const int MaxRating = 5;
    public const int MaxReviewUpdateDays = 30;
    
    // Note constraints
    public const int MaxNoteLength = 1000;
    
    // Hourly rate constraints
    public const decimal MinHourlyRate = 5.00m;
    public const decimal MaxHourlyRate = 1000.00m;
}
