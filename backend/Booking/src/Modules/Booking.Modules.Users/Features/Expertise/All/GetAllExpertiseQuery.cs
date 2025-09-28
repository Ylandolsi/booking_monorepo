using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Expertise.All;

public record GetAllExpertiseQuery : IQuery<List<ExpertiseResponse>>;