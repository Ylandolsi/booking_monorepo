using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.GetAll;

public record GetAllPayoutsAdminQuery() : IQuery<List<Domain.Entities.Payout>>;