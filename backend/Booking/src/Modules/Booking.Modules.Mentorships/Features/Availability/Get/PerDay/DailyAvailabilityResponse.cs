using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

public sealed record DailyAvailabilityResponse(
    DateTime Date,
    string MentorSlug,
    bool IsAvailable,
    List<TimeSlotResponse> TimeSlots,
    DailySummary Summary);

public sealed record DailySummary(
    int TotalSlots,
    int AvailableSlots,
    int BookedSlots,
    decimal AvailabilityPercentage); 