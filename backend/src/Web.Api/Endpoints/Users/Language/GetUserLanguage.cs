using Application.Abstractions.Messaging;
using Application.Users.Language;
using Application.Users.Language.Get;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users.Language;

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
                Result<List<LanguageResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Language);
    }
}