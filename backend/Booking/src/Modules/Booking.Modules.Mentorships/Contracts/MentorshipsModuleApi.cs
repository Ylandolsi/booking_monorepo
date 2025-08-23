using Booking.Common.Contracts.Mentorships;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Microsoft.Extensions.DependencyInjection;
using Error = Booking.Common.Results.Error;

namespace Booking.Modules.Mentorships.Contracts;

public class MentorshipsModuleApi(IServiceProvider serviceProvider) : IMentorshipsModuleApi
{
    private IServiceProvider ServiceProvider { get; set; } = serviceProvider;

    public async Task<Result<CalendarDto>> GetUserCalendar(int userId)
    {
        var calendarService = serviceProvider.GetService<GoogleCalendarService>();
        var initResult = await calendarService.InitializeAsync(userId);

        var result = await calendarService.GetCalendarAsync();
        if (result.IsFailure)
            return Result.Failure<CalendarDto>(Error.Problem(result.Error.Code, result.Error.Description));

        var resultVal = result.Value;


        var dto = new CalendarDto
        {
            TimezoneId = resultVal.TimeZone,
        };
        return Result.Success(dto);
    }
}