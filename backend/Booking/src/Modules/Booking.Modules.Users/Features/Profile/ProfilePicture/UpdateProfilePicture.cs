using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Profile.ProfilePicture;

internal sealed class UpdateProfilePicture : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile/picture", async (
            IFormFile file,
            UserContext userContext,
            ICommandHandler<UpdateProfilePictureCommand, string> handler,
            CancellationToken cancellationToken) =>
        {
            int  userId = userContext.UserId;
            


            var command = new UpdateProfilePictureCommand(userId, file);

            Result<string> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Profile)
        .DisableAntiforgery();
    }
}
