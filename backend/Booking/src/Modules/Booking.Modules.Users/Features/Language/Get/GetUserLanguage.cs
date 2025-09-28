using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Language.Get;

internal sealed class GetUserLanguage : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetUserLanguages, async (
                string userSlug,
                IQueryHandler<GetUserLanguagesQuery, List<LanguageResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserLanguagesQuery(userSlug);
                var result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Language);
    }
}