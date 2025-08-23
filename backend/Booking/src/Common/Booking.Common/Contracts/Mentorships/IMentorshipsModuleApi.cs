using Booking.Common.Results;

namespace Booking.Common.Contracts.Mentorships;

public interface IMentorshipsModuleApi
{
    Task<Result<CalendarDto>> GetUserCalendar(int userId);
}