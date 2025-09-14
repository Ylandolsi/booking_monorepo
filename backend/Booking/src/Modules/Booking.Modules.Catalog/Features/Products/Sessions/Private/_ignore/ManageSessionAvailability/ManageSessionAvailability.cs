using Booking.Common.Messaging;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private._ignore.ManageSessionAvailability;

public record UpdateSessionAvailabilityCommand(
    int SessionProductId,
    Guid UserId,
    List<AvailabilitySlotRequest> AvailabilitySlots
) : ICommand<SessionAvailabilityResponse>;

public record AvailabilitySlotRequest(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive = true
);

public record AvailabilitySlotResponse(
    int Id,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive
);

public record SessionAvailabilityResponse(
    int SessionProductId,
    List<AvailabilitySlotResponse> AvailabilitySlots
);

public class UpdateSessionAvailabilityHandler : ICommandHandler<UpdateSessionAvailabilityCommand, SessionAvailabilityResponse>
{
    public async Task<Result<SessionAvailabilityResponse>> Handle(UpdateSessionAvailabilityCommand command, CancellationToken cancellationToken)
    {
        // TODO: Get session product from database
        // TODO: Verify user owns the store that owns this product
        // TODO: Clear existing availabilities
        // TODO: Add new availabilities
        
        // TODO: Create SessionAvailability entities and save
        // For now, create placeholder response
        var responseSlots = command.AvailabilitySlots
            .Select((slot, index) => new AvailabilitySlotResponse(
                index + 1, // Placeholder ID
                slot.DayOfWeek,
                slot.StartTime,
                slot.EndTime,
                slot.IsActive))
            .ToList();

        var response = new SessionAvailabilityResponse(
            command.SessionProductId,
            responseSlots
        );

        return Result.Success(response);
    }
}
