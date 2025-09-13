using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.SetSchedule;

public sealed record SetScheduleCommand(
    int MentorId,
    List<DayAvailability> DayAvailabilities , 
    string TimeZoneId = "Africa/Tunis") : ICommand;