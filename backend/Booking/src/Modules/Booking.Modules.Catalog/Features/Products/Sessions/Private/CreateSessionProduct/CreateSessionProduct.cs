using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct;

public record CreateSessionProductCommand(
    int UserId,
    string Title,
    string Subtitle,
    string Description,
    string ClickToPay,
    decimal Price,
    int BufferTimeMinutes,
    string MeetingInstructions,
    string TimeZoneId = "Africa/Tunis"
) : ICommand<SessionProductResponse>;

public record SessionProductResponse(
    string Title,
    string Subtitle,
    string Description,
    string ClickToPay,
    decimal Price,
    string MeetingInstructions
);

public class CreateSessionProductHandler(CatalogDbContext context, ILogger<CreateSessionProductCommand> logger)
    : ICommandHandler<CreateSessionProductCommand, SessionProductResponse>
{
    public async Task<Result<SessionProductResponse>> Handle(CreateSessionProductCommand command,
        CancellationToken cancellationToken)
    {
        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

        if (store is null)
        {
        }


        var bufferTimeResult = Duration.Create(command.BufferTimeMinutes);
        if (bufferTimeResult.IsFailure)
        {
            return Result.Failure<SessionProductResponse>(bufferTimeResult.Error);
        }

        // Create the session product
        var sessionProduct = SessionProduct.Create(
            command.Title,
            command.Subtitle,
            command.Description,
            command.ClickToPay,
            command.MeetingInstructions,
            command.Price,
            bufferTimeResult.Value,
            command.TimeZoneId
        );

        await context.AddAsync(sessionProduct, cancellationToken);


        var response = new SessionProductResponse(
            sessionProduct.Title,
            sessionProduct.Subtitle,
            sessionProduct.Description,
            sessionProduct.ClickToPay,
            sessionProduct.Price,
            sessionProduct.MeetingInstructions
        );

        return Result.Success(response);
    }
}