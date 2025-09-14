using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private._ignore.SetSchedule;

public sealed record SetScheduleCommand(
    int MentorId,
    List<DayAvailability> DayAvailabilities , 
    string TimeZoneId = "Africa/Tunis") : ICommand;