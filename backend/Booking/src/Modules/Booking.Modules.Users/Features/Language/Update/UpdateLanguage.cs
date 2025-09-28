using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Language.Update;

internal sealed class UpdateLanguage : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersEndpoints.UpdateUserLanguages, async (
                Request request,
                UserContext userContext,
                ICommandHandler<UpdateUserLanguagesCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var command = new UpdateUserLanguagesCommand(userId, request.LanguageIds);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Language);
    }

    public sealed record Request(List<int>? LanguageIds);
}