using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Profile.ProfilePicture;

internal sealed class UpdateProfilePicture : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile/picture", async (
                [FromForm] IFormFile file,
                UserContext userContext,
                ICommandHandler<UpdateProfilePictureCommand, ProfilePictureRespone> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;
                var command = new UpdateProfilePictureCommand(userId, file);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    dto => Results.Ok(dto),
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Profile)
            .DisableAntiforgery();
    }
}