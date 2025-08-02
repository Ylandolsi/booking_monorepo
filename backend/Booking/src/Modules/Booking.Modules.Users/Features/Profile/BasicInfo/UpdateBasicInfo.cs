using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Profile.BasicInfo;

internal sealed class UpdateBasicInfo : IEndpoint
{
    public sealed record Request(
        string FirstName,
        string LastName,
        string Gender,
        string? Bio);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile/basic-info", async (
                Request request,
                UserContext userContext,
                ICommandHandler<UpdateBasicInfoCommand> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;


                var command = new UpdateBasicInfoCommand(
                    userId,
                    request.FirstName,
                    request.LastName,
                    request.Gender,
                    request.Bio);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Profile);
    }
}