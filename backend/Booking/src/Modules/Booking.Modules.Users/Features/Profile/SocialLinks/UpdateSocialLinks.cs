using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Profile.SocialLinks;

internal sealed class UpdateSocialLinks : IEndpoint
{
    public sealed record Request(
        string? LinkedIn,
        string? Twitter,
        string? Github,
        string? Youtube,
        string? Facebook,
        string? Instagram,
        string? Portfolio);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile/social-links", async (
            Request request,
            UserContext userContext,
            ICommandHandler<UpdateSocialLinksCommand> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;


            var command = new UpdateSocialLinksCommand(
                userId,
                request.LinkedIn,
                request.Twitter,
                request.Github,
                request.Youtube,
                request.Facebook,
                request.Instagram,
                request.Portfolio);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Profile);
    }
}
