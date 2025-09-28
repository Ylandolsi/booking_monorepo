using Booking.Common.Contracts.Mentorships;
using Booking.Modules.Mentorships.refactored.Domain.Entities;
using Booking.Modules.Mentorships.refactored.Features.GoogleCalendar;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Mentorships.refactored.Contracts;

public class MentorshipsModuleApi(IServiceProvider serviceProvider) : IMentorshipsModuleApi, IMentorshipsModuleApi
{
    private IServiceProvider ServiceProvider { get; set; } = serviceProvider;

    public async Task<Result<CalendarDto>> GetUserCalendar(int userId)
    {
        using var scope = ServiceProvider.CreateScope();

        var calendarService = scope.ServiceProvider.GetService<GoogleCalendarService>();
        var initResult = await calendarService.InitializeAsync(userId);

        var result = await calendarService.GetCalendarAsync();
        if (result.IsFailure)
            return Result.Failure<CalendarDto>(Error.Problem(result.Error.Code, result.Error.Description));

        var resultVal = result.Value;


        var dto = new CalendarDto
        {
            TimeZoneId = resultVal.TimeZone,
        };
        return Result.Success(dto);
    }

    public async Task<Result> CreateWalletForUserId(int userId , CancellationToken cancellationToken = default)
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
        var existingWallet = await dbContext.Wallets.FirstOrDefaultAsync(e => e.UserId == userId , cancellationToken);
        if (existingWallet == null)
        {
            await dbContext.AddAsync(new Wallet(userId) , cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}